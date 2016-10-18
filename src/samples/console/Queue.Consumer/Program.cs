﻿using Abp;
using Abp.Dependency;
using Abp.Net.Sockets;
using Abp.Schedule;
using BodeAbp.Queue.Clients.Consumers;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using Castle.Core.Logging;
using System;
using System.Configuration;
using System.Net;
using System.Threading;

namespace Queue.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bootstrapper = new AbpBootstrapper())
            {
                bootstrapper.Initialize();

                var address = ConfigurationManager.AppSettings["BrokerAddress"];
                var brokerAddress = string.IsNullOrEmpty(address) ? SocketUtils.GetLocalIPV4() : IPAddress.Parse(address);
                var clientCount = int.Parse(ConfigurationManager.AppSettings["ClientCount"]);
                var setting = new ConsumerSetting
                {
                    ConsumeFromWhere = ConsumeFromWhere.FirstOffset,
                    MessageHandleMode = MessageHandleMode.Parallel,
                    BrokerAddress = new IPEndPoint(brokerAddress, 5001),
                    BrokerAdminAddress = new IPEndPoint(brokerAddress, 5002)
                };
                var messageHandler = new MessageHandler();
                for (var i = 1; i <= clientCount; i++)
                {
                    new BodeAbp.Queue.Clients.Consumers.Consumer(ConfigurationManager.AppSettings["ConsumerGroup"], setting)
                        .Subscribe(ConfigurationManager.AppSettings["Topic"])
                        .SetMessageHandler(messageHandler)
                        .Start();
                }
                Console.ReadLine();
            }
        }
        class MessageHandler : IMessageHandler
        {
            private long _previusHandledCount;
            private long _handledCount;
            private long _calculateCount = 0;
            private IScheduleService _scheduleService;
            private IRTStatisticService _rtStatisticService;
            private ILogger _logger;

            public MessageHandler()
            {
                _scheduleService = IocManager.Instance.Resolve<IScheduleService>();
                _scheduleService.StartTask("PrintThroughput", PrintThroughput, 1000, 1000);
                _logger = IocManager.Instance.Resolve<ILoggerFactory>().Create(typeof(Program).Name);
                _rtStatisticService = IocManager.Instance.Resolve<IRTStatisticService>();
            }

            public void Handle(QueueMessage message, IMessageContext context)
            {
                Interlocked.Increment(ref _handledCount);
                _rtStatisticService.AddRT((DateTime.Now - message.CreatedTime).TotalMilliseconds);
                context.OnMessageHandled(message);
            }

            private void PrintThroughput()
            {
                var totalHandledCount = _handledCount;
                var throughput = totalHandledCount - _previusHandledCount;
                _previusHandledCount = totalHandledCount;
                if (throughput > 0)
                {
                    _calculateCount++;
                }

                var average = 0L;
                if (_calculateCount > 0)
                {
                    average = totalHandledCount / _calculateCount;
                }

                _logger.InfoFormat("totalReceived: {0}, throughput: {1}/s, average: {2}, delay: {3:F3}ms", totalHandledCount, throughput, average, _rtStatisticService.ResetAndGetRTStatisticInfo());
            }
        }
    }
}
