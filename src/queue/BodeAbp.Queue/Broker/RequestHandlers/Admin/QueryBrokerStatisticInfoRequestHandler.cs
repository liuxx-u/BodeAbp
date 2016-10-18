using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Utils;
using EQueue.Broker.Exceptions;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class QueryBrokerStatisticInfoRequestHandler : IRequestHandler
    {
        private ISerializer<byte[]> _binarySerializer;

        public QueryBrokerStatisticInfoRequestHandler()
        {
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                throw new BrokerCleanningException();
            }
            var statisticInfo = BrokerController.Instance.GetBrokerStatisticInfo();
            return RemotingResponseFactory.CreateResponse(remotingRequest, _binarySerializer.Serialize(statisticInfo));
        }
    }
}
