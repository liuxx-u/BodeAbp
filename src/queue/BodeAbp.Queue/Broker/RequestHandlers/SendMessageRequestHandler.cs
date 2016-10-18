﻿using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Net.Sockets.Buffer;
using BodeAbp.Queue.Broker.LongPolling;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using Castle.Core.Logging;
using EQueue.Broker.Exceptions;
using System;
using System.Text;

namespace BodeAbp.Queue.Broker.RequestHandlers
{
    public class SendMessageRequestHandler : IRequestHandler
    {
        private readonly SuspendedPullRequestManager _suspendedPullRequestManager;
        private readonly IMessageStore _messageStore;
        private readonly IQueueStore _queueStore;
        private readonly ILogger _logger;
        private readonly ILogger _sendRTLogger;
        private readonly object _syncObj = new object();
        private readonly BrokerController _brokerController;
        private readonly bool _notifyWhenMessageArrived;
        private readonly BufferQueue<StoreContext> _bufferQueue;
        private const string SendMessageFailedText = "Send message failed.";

        public SendMessageRequestHandler(BrokerController brokerController)
        {
            _brokerController = brokerController;
            _suspendedPullRequestManager = IocManager.Instance.Resolve<SuspendedPullRequestManager>();
            _messageStore = IocManager.Instance.Resolve<IMessageStore>();
            _queueStore = IocManager.Instance.Resolve<IQueueStore>();
            _notifyWhenMessageArrived = _brokerController.Setting.NotifyWhenMessageArrived;
            _logger = IocManager.Instance.Resolve<ILoggerFactory>().Create(GetType().FullName);
            _sendRTLogger = IocManager.Instance.Resolve<ILoggerFactory>().Create("SendRT");
            var messageWriteQueueThreshold = brokerController.Setting.MessageWriteQueueThreshold;
            _bufferQueue = new BufferQueue<StoreContext>("QueueBufferQueue", messageWriteQueueThreshold, OnQueueMessageCompleted, _logger);
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (remotingRequest.Body.Length > _brokerController.Setting.MessageMaxSize)
            {
                throw new Exception("Message size cannot exceed max message size:" + _brokerController.Setting.MessageMaxSize);
            }

            if (BrokerController.Instance.IsCleaning)
            {
                throw new BrokerCleanningException();
            }

            var request = MessageUtils.DecodeSendMessageRequest(remotingRequest.Body);
            var message = request.Message;
            var queueId = request.QueueId;
            var queue = _queueStore.GetQueue(message.Topic, queueId);
            if (queue == null)
            {
                throw new QueueNotExistException(message.Topic, queueId);
            }

            _messageStore.StoreMessageAsync(queue, message, (record, parameter) =>
            {
                var storeContext = parameter as StoreContext;
                storeContext.Queue.AddMessage(record.LogPosition, record.Tag);
                storeContext.MessageLogRecord = record;
                _bufferQueue.EnqueueMessage(storeContext);
            }, new StoreContext
            {
                RequestHandlerContext = context,
                RemotingRequest = remotingRequest,
                Queue = queue,
                SendMessageRequestHandler = this
            });

            return null;
        }

        private void OnQueueMessageCompleted(StoreContext storeContext)
        {
            storeContext.OnComplete();
        }
        class StoreContext
        {
            public IRequestHandlerContext RequestHandlerContext;
            public RemotingRequest RemotingRequest;
            public Queue Queue;
            public MessageLogRecord MessageLogRecord;
            public SendMessageRequestHandler SendMessageRequestHandler;

            public void OnComplete()
            {
                if (MessageLogRecord.LogPosition >= 0 && !string.IsNullOrEmpty(MessageLogRecord.MessageId))
                {
                    var result = new MessageStoreResult(
                        MessageLogRecord.MessageId,
                        MessageLogRecord.Code,
                        MessageLogRecord.Topic,
                        MessageLogRecord.QueueId,
                        MessageLogRecord.QueueOffset,
                        MessageLogRecord.CreatedTime,
                        MessageLogRecord.StoredTime,
                        MessageLogRecord.Tag);
                    var data = MessageUtils.EncodeMessageStoreResult(result);
                    var response = RemotingResponseFactory.CreateResponse(RemotingRequest, data);

                    RequestHandlerContext.SendRemotingResponse(response);

                    if (SendMessageRequestHandler._notifyWhenMessageArrived)
                    {
                        SendMessageRequestHandler._suspendedPullRequestManager.NotifyNewMessage(MessageLogRecord.Topic, result.QueueId, result.QueueOffset);
                    }
                }
                else
                {
                    var response = RemotingResponseFactory.CreateResponse(RemotingRequest, ResponseCode.Failed, Encoding.UTF8.GetBytes(SendMessageFailedText));
                    RequestHandlerContext.SendRemotingResponse(response);
                }
            }
        }
    }
}
