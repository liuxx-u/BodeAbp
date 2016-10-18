﻿using Abp;
using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Net.Sockets;
using Abp.Schedule;
using BodeAbp.Queue.Clients.Producers;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Queue.Producer
{
    class Program
    {
        static long _previousSentCount = 0;
        static long _sentCount = 0;
        static long _calculateCount = 0;
        static string _mode;
        static bool _hasError;
        static ILogger _logger;
        static IScheduleService _scheduleService;
        static IRTStatisticService _rtStatisticService;

        static void Main(string[] args)
        {
            using (var bootstrapper = new AbpBootstrapper())
            {
                bootstrapper.Initialize();

                _logger = IocManager.Instance.Resolve<ILoggerFactory>().Create(typeof(Program).Name);
                _scheduleService = IocManager.Instance.Resolve<IScheduleService>();
                _rtStatisticService = IocManager.Instance.Resolve<IRTStatisticService>();
                
                SendMessageTest();
                StartPrintThroughputTask();
                Console.ReadLine();
            }
        }
        static void SendMessageTest()
        {
            _mode = ConfigurationManager.AppSettings["Mode"];

            var address = ConfigurationManager.AppSettings["BrokerAddress"];
            var brokerAddress = string.IsNullOrEmpty(address) ? SocketUtils.GetLocalIPV4() : IPAddress.Parse(address);
            var clientCount = int.Parse(ConfigurationManager.AppSettings["ClientCount"]);
            var messageSize = int.Parse(ConfigurationManager.AppSettings["MessageSize"]);
            var messageCount = int.Parse(ConfigurationManager.AppSettings["MessageCount"]);
            var actions = new List<Action>();
            var payload = new byte[messageSize];
            var topic = ConfigurationManager.AppSettings["Topic"];

            for (var i = 0; i < clientCount; i++)
            {
                var setting = new ProducerSetting
                {
                    BrokerAddress = new IPEndPoint(brokerAddress, 5000),
                    BrokerAdminAddress = new IPEndPoint(brokerAddress, 5002)
                };
                var producer = new BodeAbp.Queue.Clients.Producers.Producer(setting).Start();
                actions.Add(() => SendMessages(producer, _mode, messageCount, topic, payload));
            }

            Task.Factory.StartNew(() => Parallel.Invoke(actions.ToArray()));
        }
        static void SendMessages(BodeAbp.Queue.Clients.Producers.Producer producer, string mode, int messageCount, string topic, byte[] payload)
        {
            _logger.Info("----Send message starting----");

            var sendAction = default(Action<int>);

            if (_mode == "Oneway")
            {
                sendAction = index =>
                {
                    var message = new Message(topic, 100, payload);
                    producer.SendOneway(message, index.ToString());
                    _rtStatisticService.AddRT((DateTime.Now - message.CreatedTime).TotalMilliseconds);
                    Interlocked.Increment(ref _sentCount);
                };
            }
            else if (_mode == "Sync")
            {
                sendAction = index =>
                {
                    var message = new Message(topic, 100, payload);
                    var result = producer.Send(message, index.ToString());
                    if (result.SendStatus != SendStatus.Success)
                    {
                        throw new Exception(result.ErrorMessage);
                    }
                    _rtStatisticService.AddRT((DateTime.Now - message.CreatedTime).TotalMilliseconds);
                    Interlocked.Increment(ref _sentCount);
                };
            }
            else if (_mode == "Async")
            {
                sendAction = index =>
                {
                    var message = new Message(topic, 100, payload);
                    producer.SendAsync(message, index.ToString()).ContinueWith(t =>
                    {
                        if (t.Exception != null)
                        {
                            _hasError = true;
                            _logger.ErrorFormat("Send message has exception, errorMessage: {0}", t.Exception.GetBaseException().Message);
                            return;
                        }
                        if (t.Result == null)
                        {
                            _hasError = true;
                            _logger.Error("Send message timeout.");
                            return;
                        }
                        if (t.Result.SendStatus != SendStatus.Success)
                        {
                            _hasError = true;
                            _logger.ErrorFormat("Send message failed, errorMessage: {0}", t.Result.ErrorMessage);
                            return;
                        }
                        _rtStatisticService.AddRT((DateTime.Now - message.CreatedTime).TotalMilliseconds);
                        Interlocked.Increment(ref _sentCount);
                    });
                };
            }
            else if (_mode == "Callback")
            {
                producer.RegisterResponseHandler(new ResponseHandler());
                sendAction = index =>
                {
                    var message = new Message(topic, 100, payload);
                    producer.SendWithCallback(message, index.ToString());
                };
            }

            Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < messageCount; i++)
                {
                    try
                    {
                        sendAction(i);
                    }
                    catch (Exception ex)
                    {
                        _hasError = true;
                        _logger.ErrorFormat("Send message failed, errorMsg:{0}", ex.Message);
                    }

                    if (_hasError)
                    {
                        Thread.Sleep(3000);
                        _hasError = false;
                    }
                }
            });
        }

        static void StartPrintThroughputTask()
        {
            _scheduleService.StartTask("PrintThroughput", PrintThroughput, 1000, 1000);
        }
        static void PrintThroughput()
        {
            var totalSentCount = _sentCount;
            var throughput = totalSentCount - _previousSentCount;
            _previousSentCount = totalSentCount;
            if (throughput > 0)
            {
                _calculateCount++;
            }

            var average = 0L;
            if (_calculateCount > 0)
            {
                average = totalSentCount / _calculateCount;
            }
            _logger.InfoFormat("Send message mode: {0}, totalSent: {1}, throughput: {2}/s, average: {3}, rt: {4:F3}ms", _mode, totalSentCount, throughput, average, _rtStatisticService.ResetAndGetRTStatisticInfo());
        }

        class ResponseHandler : IResponseHandler
        {
            public void HandleResponse(RemotingResponse remotingResponse)
            {
                var sendResult = BodeAbp.Queue.Clients.Producers.Producer.ParseSendResult(remotingResponse);
                if (sendResult.SendStatus != SendStatus.Success)
                {
                    _hasError = true;
                    _logger.Error(sendResult.ErrorMessage);
                    return;
                }
                Interlocked.Increment(ref _sentCount);
                _rtStatisticService.AddRT((DateTime.Now - sendResult.MessageStoreResult.CreatedTime).TotalMilliseconds);
            }
        }
    }
}
