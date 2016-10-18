using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Broker.Client;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using EQueue.Broker.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class QueryConsumerInfoRequestHandler : IRequestHandler
    {
        private ISerializer<byte[]> _binarySerializer;
        private IConsumeOffsetStore _offsetStore;
        private ConsumerManager _consumerManager;
        private IQueueStore _queueStore;

        public QueryConsumerInfoRequestHandler()
        {
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
            _offsetStore = IocManager.Instance.Resolve<IConsumeOffsetStore>();
            _consumerManager = IocManager.Instance.Resolve<ConsumerManager>();
            _queueStore = IocManager.Instance.Resolve<IQueueStore>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                throw new BrokerCleanningException();
            }

            var request = _binarySerializer.Deserialize<byte[],QueryConsumerInfoRequest>(remotingRequest.Body);
            var consumerInfoList = new List<ConsumerInfo>();
            var consumerGroups = default(IEnumerable<ConsumerGroup>);
            var allConsumerGroupNames = _offsetStore.GetAllConsumerGroupNames();

            if (!string.IsNullOrEmpty(request.GroupName))
            {
                consumerGroups = _consumerManager.QueryConsumerGroup(request.GroupName);
            }
            else
            {
                consumerGroups = _consumerManager.GetAllConsumerGroups();
            }

            var notEmptyConsumerGroups = _consumerManager.GetAllConsumerGroups().Where(x => x.GetConsumerCount() > 0);
            var emptyConsumerGroups = allConsumerGroupNames.Where(x => notEmptyConsumerGroups.Count() == 0 || !notEmptyConsumerGroups.Any(y => y.GroupName == x));

            foreach (var groupName in emptyConsumerGroups)
            {
                consumerInfoList.Add(BuildConsumerInfoForEmptyGroup(groupName));
            }

            foreach (var consumerGroup in consumerGroups)
            {
                consumerInfoList.AddRange(GetConsumerInfoForGroup(consumerGroup, request.Topic));
            }

            return RemotingResponseFactory.CreateResponse(remotingRequest, _binarySerializer.Serialize(consumerInfoList));
        }

        private IEnumerable<ConsumerInfo> GetConsumerInfoForGroup(ConsumerGroup consumerGroup, string currentTopic)
        {
            var consumerInfoList = new List<ConsumerInfo>();
            var consumerIdList = string.IsNullOrEmpty(currentTopic) ? consumerGroup.GetAllConsumerIds().ToList() : consumerGroup.QueryConsumerIdsForTopic(currentTopic).ToList();
            consumerIdList.Sort();

            foreach (var consumerId in consumerIdList)
            {
                var consumingQueues = consumerGroup.GetConsumingQueue(consumerId);
                foreach (var consumingQueue in consumingQueues)
                {
                    if (string.IsNullOrEmpty(currentTopic) || consumingQueue.Topic.Contains(currentTopic))
                    {
                        consumerInfoList.Add(BuildConsumerInfo(consumerGroup.GroupName, consumerId, consumingQueue.Topic, consumingQueue.QueueId));
                    }
                }
            }

            consumerInfoList.Sort((x, y) =>
            {
                var result = string.Compare(x.ConsumerGroup, y.ConsumerGroup);
                if (result != 0)
                {
                    return result;
                }
                result = string.Compare(x.ConsumerId, y.ConsumerId);
                if (result != 0)
                {
                    return result;
                }
                result = string.Compare(x.Topic, y.Topic);
                if (result != 0)
                {
                    return result;
                }
                if (x.QueueId > y.QueueId)
                {
                    return 1;
                }
                else if (x.QueueId < y.QueueId)
                {
                    return -1;
                }
                return 0;
            });

            return consumerInfoList;
        }
        private ConsumerInfo BuildConsumerInfoForEmptyGroup(string group)
        {
            var consumerInfo = new ConsumerInfo();
            consumerInfo.ConsumerGroup = group;
            consumerInfo.QueueId = -1;
            consumerInfo.QueueCurrentOffset = -1L;
            consumerInfo.ConsumedOffset = -1L;
            return consumerInfo;
        }
        private ConsumerInfo BuildConsumerInfo(string group, string consumerId, string topic, int queueId)
        {
            var queueCurrentOffset = _queueStore.GetQueueCurrentOffset(topic, queueId);
            var consumerInfo = new ConsumerInfo();
            consumerInfo.ConsumerGroup = group;
            consumerInfo.ConsumerId = consumerId;
            consumerInfo.Topic = topic;
            consumerInfo.QueueId = queueId;
            consumerInfo.QueueCurrentOffset = queueCurrentOffset;
            consumerInfo.ConsumedOffset = _offsetStore.GetConsumeOffset(topic, queueId, group);
            return consumerInfo;
        }
    }
}
