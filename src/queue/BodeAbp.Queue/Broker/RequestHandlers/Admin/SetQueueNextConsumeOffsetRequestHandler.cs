using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Broker.LongPolling;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class SetQueueNextConsumeOffsetRequestHandler : IRequestHandler
    {
        private readonly IConsumeOffsetStore _offsetStore;
        private readonly ISerializer<byte[]> _binarySerializer;
        private readonly SuspendedPullRequestManager _suspendedPullRequestManager;

        public SetQueueNextConsumeOffsetRequestHandler()
        {
            _offsetStore = IocManager.Instance.Resolve<IConsumeOffsetStore>();
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
            _suspendedPullRequestManager = IocManager.Instance.Resolve<SuspendedPullRequestManager>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                return null;
            }

            var request = _binarySerializer.Deserialize<byte[], SetQueueNextConsumeOffsetRequest>(remotingRequest.Body);
            _offsetStore.SetConsumeNextOffset(
                request.Topic,
                request.QueueId,
                request.ConsumerGroup,
                request.NextOffset);
            _suspendedPullRequestManager.RemovePullRequest(request.ConsumerGroup, request.Topic, request.QueueId);

            return RemotingResponseFactory.CreateResponse(remotingRequest);
        }
    }
}
