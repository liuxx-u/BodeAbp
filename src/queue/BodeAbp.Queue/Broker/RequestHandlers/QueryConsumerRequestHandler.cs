using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Broker.Client;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodeAbp.Queue.Broker.RequestHandlers
{
    public class QueryConsumerRequestHandler : IRequestHandler
    {
        private ConsumerManager _consumerManager;
        private ISerializer<byte[]> _binarySerializer;

        public QueryConsumerRequestHandler()
        {
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
            _consumerManager = IocManager.Instance.Resolve<ConsumerManager>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                return RemotingResponseFactory.CreateResponse(remotingRequest, Encoding.UTF8.GetBytes(string.Empty));
            }

            var request = _binarySerializer.Deserialize<byte[], QueryConsumerRequest>(remotingRequest.Body);
            var consumerGroup = _consumerManager.GetConsumerGroup(request.GroupName);
            var consumerIdList = new List<string>();
            if (consumerGroup != null)
            {
                consumerIdList = consumerGroup.GetConsumerIdsForTopic(request.Topic).ToList();
                consumerIdList.Sort();
            }
            return RemotingResponseFactory.CreateResponse(remotingRequest, Encoding.UTF8.GetBytes(string.Join(",", consumerIdList)));
        }
    }
}
