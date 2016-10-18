using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Broker.Client;
using BodeAbp.Queue.Protocols;

namespace BodeAbp.Queue.Broker.RequestHandlers
{
    public class ConsumerHeartbeatRequestHandler : IRequestHandler
    {
        private ConsumerManager _consumerManager;
        private ISerializer<byte[]> _binarySerializer;

        public ConsumerHeartbeatRequestHandler(BrokerController brokerController)
        {
            _consumerManager = IocManager.Instance.Resolve<ConsumerManager>();
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            var consumerData = _binarySerializer.Deserialize<byte[], ConsumerHeartbeatData>(remotingRequest.Body);
            _consumerManager.RegisterConsumer(consumerData.GroupName, consumerData.ConsumerId, consumerData.SubscriptionTopics, consumerData.ConsumingQueues, context.Connection);
            return null;
        }
    }
}
