using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Protocols;

namespace BodeAbp.Queue.Broker.RequestHandlers
{
    public class UpdateQueueOffsetRequestHandler : IRequestHandler
    {
        private IConsumeOffsetStore _offsetStore;
        private ISerializer<byte[]> _binarySerializer;

        public UpdateQueueOffsetRequestHandler()
        {
            _offsetStore = IocManager.Instance.Resolve<IConsumeOffsetStore>();
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                return null;
            }

            var request = _binarySerializer.Deserialize<byte[],UpdateQueueOffsetRequest>(remotingRequest.Body);
            _offsetStore.UpdateConsumeOffset(
                request.MessageQueue.Topic,
                request.MessageQueue.QueueId,
                request.QueueOffset,
                request.ConsumerGroup);
            return null;
        }
    }
}
