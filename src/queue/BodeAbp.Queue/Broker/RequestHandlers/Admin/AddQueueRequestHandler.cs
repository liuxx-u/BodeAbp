using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using EQueue.Broker.Exceptions;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class AddQueueRequestHandler : IRequestHandler
    {
        private ISerializer<byte[]> _binarySerializer;
        private IQueueStore _queueStore;

        public AddQueueRequestHandler()
        {
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
            _queueStore = IocManager.Instance.Resolve<IQueueStore>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                throw new BrokerCleanningException();
            }
            var addQueueRequest = _binarySerializer.Deserialize<byte[], AddQueueRequest>(remotingRequest.Body);
            _queueStore.AddQueue(addQueueRequest.Topic);
            return RemotingResponseFactory.CreateResponse(remotingRequest);
        }
    }
}
