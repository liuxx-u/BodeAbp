using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using EQueue.Broker.Exceptions;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class DeleteQueueRequestHandler : IRequestHandler
    {
        private ISerializer<byte[]> _binarySerializer;
        private IQueueStore _queueStore;

        public DeleteQueueRequestHandler()
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
            var request = _binarySerializer.Deserialize<byte[], DeleteQueueRequest>(remotingRequest.Body);
            _queueStore.DeleteQueue(request.Topic, request.QueueId);
            return RemotingResponseFactory.CreateResponse(remotingRequest);
        }
    }
}
