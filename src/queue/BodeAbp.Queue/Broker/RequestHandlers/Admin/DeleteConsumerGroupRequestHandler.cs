using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Broker.Client;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using EQueue.Broker.Exceptions;
using System;
using System.Text;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class DeleteConsumerGroupRequestHandler : IRequestHandler
    {
        private ISerializer<byte[]> _binarySerializer;
        private IConsumeOffsetStore _offsetStore;
        private ConsumerManager _consumerManager;

        public DeleteConsumerGroupRequestHandler()
        {
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
            _offsetStore = IocManager.Instance.Resolve<IConsumeOffsetStore>();
            _consumerManager = IocManager.Instance.Resolve<ConsumerManager>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                throw new BrokerCleanningException();
            }

            var request = _binarySerializer.Deserialize<byte[], DeleteConsumerGroupRequest>(remotingRequest.Body);

            if (string.IsNullOrEmpty(request.GroupName))
            {
                throw new ArgumentException("DeleteConsumerGroupRequest.GroupName cannot be null or empty.");
            }
            var consumerGroup = _consumerManager.GetConsumerGroup(request.GroupName);
            if (consumerGroup != null && consumerGroup.GetConsumerCount() > 0)
            {
                throw new Exception("Consumer group has consumer exist, not allowed to delete.");
            }

            var success = _offsetStore.DeleteConsumerGroup(request.GroupName);

            return RemotingResponseFactory.CreateResponse(remotingRequest, Encoding.UTF8.GetBytes(success ? "1" : "0"));
        }
    }
}
