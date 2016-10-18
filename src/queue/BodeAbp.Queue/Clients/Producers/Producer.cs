﻿using Abp.Dependency;
using Abp.Extensions;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Net.Sockets;
using Abp.Schedule;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using Castle.Core.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BodeAbp.Queue.Clients.Producers
{
    public class Producer
    {
        private readonly object _lockObject = new object();
        private readonly byte[] EmptyBytes = new byte[0];
        private readonly ConcurrentDictionary<string, IList<int>> _topicQueueIdsDict;
        private readonly IScheduleService _scheduleService;
        private readonly SocketRemotingClient _remotingClient;
        private readonly SocketRemotingClient _adminRemotingClient;
        private readonly IQueueSelector _queueSelector;
        private readonly ILogger _logger;
        private bool _started;

        public string Name { get; private set; }
        public ProducerSetting Setting { get; private set; }

        public Producer(string name = null) : this(null, name) { }
        public Producer(ProducerSetting setting = null, string name = null)
        {
            Name = name;
            Setting = setting ?? new ProducerSetting();

            _topicQueueIdsDict = new ConcurrentDictionary<string, IList<int>>();
            _remotingClient = new SocketRemotingClient(Setting.BrokerAddress, Setting.SocketSetting, Setting.LocalAddress);
            _adminRemotingClient = new SocketRemotingClient(Setting.BrokerAdminAddress, Setting.SocketSetting, Setting.LocalAdminAddress);
            _scheduleService = IocManager.Instance.Resolve<IScheduleService>();
            _queueSelector = IocManager.Instance.Resolve<IQueueSelector>();
            _logger = IocManager.Instance.Resolve<ILoggerFactory>().Create(GetType().FullName);

            _adminRemotingClient.RegisterConnectionEventListener(new ConnectionEventListener(this));
        }

        public Producer RegisterConnectionEventListener(IConnectionEventListener listener)
        {
            _remotingClient.RegisterConnectionEventListener(listener);
            return this;
        }
        public Producer RegisterResponseHandler(IResponseHandler responseHandler)
        {
            _remotingClient.RegisterResponseHandler((int)RequestCode.SendMessage, responseHandler);
            return this;
        }
        public Producer Start()
        {
            _remotingClient.Start();
            _adminRemotingClient.Start();
            _started = true;
            _logger.InfoFormat("Producer started, local address: {0}", _remotingClient.LocalEndPoint);
            return this;
        }
        public Producer Shutdown()
        {
            _remotingClient.Shutdown();
            _adminRemotingClient.Shutdown();
            _logger.Info("Producer shutdown");
            return this;
        }
        public SendResult Send(Message message, string routingKey, int timeoutMilliseconds = 120000)
        {
            EnsureRemotingClientStatus();
            var sendResult = SendAsync(message, routingKey, timeoutMilliseconds).WaitResult<SendResult>(timeoutMilliseconds + 1000);
            if (sendResult == null)
            {
                sendResult = new SendResult(SendStatus.Timeout, null, string.Format("Send message timeout, message: {0}, routingKey: {1}, timeoutMilliseconds: {2}", message, routingKey, timeoutMilliseconds));
            }
            return sendResult;
        }
        public async Task<SendResult> SendAsync(Message message, string routingKey, int timeoutMilliseconds = 120000)
        {
            message.CheckNotNull("message");
            EnsureRemotingClientStatus();
            var queueId = GetAvailableQueueId(message, routingKey);
            if (queueId < 0)
            {
                throw new Exception(string.Format("No available routing queue for topic [{0}].", message.Topic));
            }
            var remotingRequest = BuildSendMessageRequest(message, queueId);

            try
            {
                var remotingResponse = await _remotingClient.InvokeAsync(remotingRequest, timeoutMilliseconds).ConfigureAwait(false);

                if (remotingResponse == null)
                {
                    return new SendResult(SendStatus.Timeout, null, string.Format("Send message timeout, message: {0}, routingKey: {1}, timeoutMilliseconds: {2}", message, routingKey, timeoutMilliseconds));
                }

                return ParseSendResult(remotingResponse);
            }
            catch (Exception ex)
            {
                return new SendResult(SendStatus.Failed, null, ex.Message);
            }
        }
        public void SendWithCallback(Message message, string routingKey)
        {
            message.CheckNotNull("message");
            EnsureRemotingClientStatus();
            var queueId = GetAvailableQueueId(message, routingKey);
            if (queueId < 0)
            {
                throw new Exception(string.Format("No available routing queue for topic [{0}].", message.Topic));
            }
            var remotingRequest = BuildSendMessageRequest(message, queueId);

            _remotingClient.InvokeWithCallback(remotingRequest);
        }
        public void SendOneway(Message message, string routingKey)
        {
            message.CheckNotNull("message");
            EnsureRemotingClientStatus();
            var queueId = GetAvailableQueueId(message, routingKey);
            if (queueId < 0)
            {
                throw new Exception(string.Format("No available routing queue for topic [{0}].", message.Topic));
            }
            var remotingRequest = BuildSendMessageRequest(message, queueId);

            _remotingClient.InvokeOneway(remotingRequest);
        }

        public static SendResult ParseSendResult(RemotingResponse remotingResponse)
        {
            remotingResponse.CheckNotNull("remotingResponse");

            if (remotingResponse.Code == ResponseCode.Success)
            {
                var result = MessageUtils.DecodeMessageStoreResult(remotingResponse.Body);
                return new SendResult(SendStatus.Success, result, null);
            }
            else if (remotingResponse.Code == 0)
            {
                return new SendResult(SendStatus.Timeout, null, Encoding.UTF8.GetString(remotingResponse.Body));
            }
            else
            {
                return new SendResult(SendStatus.Failed, null, Encoding.UTF8.GetString(remotingResponse.Body));
            }
        }

        private string GetProducerId()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return ClientIdFactory.CreateClientId(_remotingClient.LocalEndPoint as IPEndPoint);
            }
            else
            {
                var ipSuffix = ClientIdFactory.CreateClientId(_remotingClient.LocalEndPoint as IPEndPoint);
                return string.Format("{0}_{1}", Name, ipSuffix);
            }
        }
        private void SendHeartbeat()
        {
            try
            {
                _remotingClient.InvokeOneway(new RemotingRequest((int)RequestCode.ProducerHeartbeat, Encoding.UTF8.GetBytes(GetProducerId())));
            }
            catch (Exception ex)
            {
                if (_remotingClient.IsConnected)
                {
                    _logger.Error("SendHeartbeat remoting request to broker has exception.", ex);
                }
            }
        }
        private int GetAvailableQueueId(Message message, string routingKey)
        {
            var queueIds = GetTopicQueueIds(message.Topic);
            if (queueIds.IsEmpty())
            {
                return -1;
            }
            return _queueSelector.SelectQueueId(queueIds, message, routingKey);
        }
        private IList<int> GetTopicQueueIds(string topic)
        {
            var queueIds = _topicQueueIdsDict.GetOrAdd(topic, k => new List<int>());
            if (queueIds.IsEmpty())
            {
                try
                {
                    var queueIdsFromServer = GetTopicQueueIdsFromServer(topic).ToList();
                    _topicQueueIdsDict[topic] = queueIdsFromServer;
                    queueIds = queueIdsFromServer;
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("GetTopicQueueIdsFromServer has exception, topic: {0}", topic), ex);
                }
            }
            return queueIds;
        }
        private void RefreshTopicQueueCount()
        {
            foreach (var topic in _topicQueueIdsDict.Keys)
            {
                UpdateTopicQueues(topic);
            }
        }
        private void UpdateTopicQueues(string topic)
        {
            try
            {
                var topicQueueIdsFromServer = GetTopicQueueIdsFromServer(topic).ToList();
                IList<int> currentQueueIds;
                var topicQueueIdsOfLocal = _topicQueueIdsDict.TryGetValue(topic, out currentQueueIds) ? currentQueueIds : new List<int>();

                if (IsIntCollectionChanged(topicQueueIdsFromServer, topicQueueIdsOfLocal))
                {
                    _topicQueueIdsDict[topic] = topicQueueIdsFromServer;
                    _logger.InfoFormat("Queues of topic changed, topic: {0}, old queueIds: {1}, new queueIds: {2}", topic, string.Join(":", topicQueueIdsOfLocal), string.Join(":", topicQueueIdsFromServer));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("UpdateTopicQueues has exception, topic: {0}", topic), ex);
            }
        }
        private IEnumerable<int> GetTopicQueueIdsFromServer(string topic)
        {
            EnsureRemotingClientStatus();
            var remotingRequest = new RemotingRequest((int)RequestCode.GetTopicQueueIdsForProducer, Encoding.UTF8.GetBytes(topic));
            var remotingResponse = _adminRemotingClient.InvokeSync(remotingRequest, 60000);
            if (remotingResponse.Code != ResponseCode.Success)
            {
                throw new Exception(string.Format("Get topic queueIds from broker failed, topic: {0}, remoting response code: {1}", topic, remotingResponse.Code));
            }

            var queueIds = Encoding.UTF8.GetString(remotingResponse.Body);
            return queueIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).OrderBy(x => x);
        }
        private RemotingRequest BuildSendMessageRequest(Message message, int queueId)
        {
            var request = new SendMessageRequest { Message = message, QueueId = queueId };
            var data = MessageUtils.EncodeSendMessageRequest(request);
            if (data.Length > Setting.MessageMaxSize)
            {
                throw new Exception("Message size cannot exceed max message size:" + Setting.MessageMaxSize);
            }
            return new RemotingRequest((int)RequestCode.SendMessage, data);
        }
        private void StartBackgroundJobs()
        {
            lock (_lockObject)
            {
                _topicQueueIdsDict.Clear();
                _scheduleService.StartTask("RefreshTopicQueueCount", RefreshTopicQueueCount, 1000, Setting.UpdateTopicQueueCountInterval);
                _scheduleService.StartTask("SendHeartbeat", SendHeartbeat, 1000, Setting.HeartbeatBrokerInterval);
            }
        }
        private void StopBackgroundJobs()
        {
            lock (_lockObject)
            {
                _scheduleService.StopTask("RefreshTopicQueueCount");
                _scheduleService.StopTask("SendHeartbeat");
                _topicQueueIdsDict.Clear();
            }
        }
        private bool IsIntCollectionChanged(IList<int> first, IList<int> second)
        {
            if (first.Count != second.Count)
            {
                return true;
            }
            for (var index = 0; index < first.Count; index++)
            {
                if (first[index] != second[index])
                {
                    return true;
                }
            }
            return false;
        }
        private void EnsureRemotingClientStatus()
        {
            if (!_started)
            {
                throw new Exception("Producer is not started, please start it first.");
            }
            if (!_remotingClient.IsConnected)
            {
                throw new Exception("The remoting client is not connected to remoting server, please check the network.");
            }
        }

        class ConnectionEventListener : IConnectionEventListener
        {
            private readonly Producer _producer;

            public ConnectionEventListener(Producer producer)
            {
                _producer = producer;
            }

            public void OnConnectionAccepted(ITcpConnection connection) { }
            public void OnConnectionFailed(SocketError socketError) { }
            public void OnConnectionClosed(ITcpConnection connection, SocketError socketError)
            {
                _producer.StopBackgroundJobs();
            }
            public void OnConnectionEstablished(ITcpConnection connection)
            {
                _producer.StartBackgroundJobs();
            }
        }
    }
}
