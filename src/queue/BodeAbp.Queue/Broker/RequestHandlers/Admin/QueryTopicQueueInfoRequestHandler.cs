using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using EQueue.Broker.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class QueryTopicQueueInfoRequestHandler : IRequestHandler
    {
        private ISerializer<byte[]> _binarySerializer;
        private IQueueStore _queueStore;
        private IConsumeOffsetStore _offsetStore;

        public QueryTopicQueueInfoRequestHandler()
        {
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
            _queueStore = IocManager.Instance.Resolve<IQueueStore>();
            _offsetStore = IocManager.Instance.Resolve<IConsumeOffsetStore>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                throw new BrokerCleanningException();
            }

            var request = _binarySerializer.Deserialize<byte[],QueryTopicQueueInfoRequest>(remotingRequest.Body);
            var topicQueueInfoList = new List<TopicQueueInfo>();
            var queues = _queueStore.QueryQueues(request.Topic).ToList().OrderBy(x => x.Topic).ThenBy(x => x.QueueId);

            foreach (var queue in queues)
            {
                var topicQueueInfo = new TopicQueueInfo();
                topicQueueInfo.Topic = queue.Topic;
                topicQueueInfo.QueueId = queue.QueueId;
                topicQueueInfo.QueueCurrentOffset = queue.NextOffset - 1;
                topicQueueInfo.QueueMinOffset = queue.GetMinQueueOffset();
                topicQueueInfo.QueueMinConsumedOffset = _offsetStore.GetMinConsumedOffset(queue.Topic, queue.QueueId);
                topicQueueInfo.ProducerVisible = queue.Setting.ProducerVisible;
                topicQueueInfo.ConsumerVisible = queue.Setting.ConsumerVisible;
                topicQueueInfoList.Add(topicQueueInfo);
            }

            return RemotingResponseFactory.CreateResponse(remotingRequest, _binarySerializer.Serialize(topicQueueInfoList));
        }
    }
}
