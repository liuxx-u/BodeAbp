using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using BodeAbp.Queue.Utils;
using System.Linq;
using System.Text;

namespace BodeAbp.Queue.Broker.RequestHandlers
{
    public class GetTopicQueueIdsForProducerRequestHandler : IRequestHandler
    {
        private IQueueStore _queueStore;

        public GetTopicQueueIdsForProducerRequestHandler()
        {
            _queueStore = IocManager.Instance.Resolve<IQueueStore>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                return RemotingResponseFactory.CreateResponse(remotingRequest, Encoding.UTF8.GetBytes(string.Empty));
            }
            var topic = Encoding.UTF8.GetString(remotingRequest.Body);
            var queueIds = _queueStore.GetQueues(topic, BrokerController.Instance.Setting.AutoCreateTopic).Where(x => x.Setting.ProducerVisible).Select(x => x.QueueId).ToList();
            var data = Encoding.UTF8.GetBytes(string.Join(",", queueIds));
            return RemotingResponseFactory.CreateResponse(remotingRequest, data);
        }
    }
}
