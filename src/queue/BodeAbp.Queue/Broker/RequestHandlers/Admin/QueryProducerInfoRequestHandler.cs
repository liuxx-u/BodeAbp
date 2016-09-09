using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Broker.Client;
using BodeAbp.Queue.Utils;
using EQueue.Broker.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class QueryProducerInfoRequestHandler : IRequestHandler
    {
        private ISerializer<byte[]> _binarySerializer;
        private ProducerManager _producerManager;

        public QueryProducerInfoRequestHandler()
        {
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
            _producerManager = IocManager.Instance.Resolve<ProducerManager>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                throw new BrokerCleanningException();
            }
            var producerIdList = _producerManager.GetAllProducers();
            var data = Encoding.UTF8.GetBytes(string.Join(",", producerIdList));
            return RemotingResponseFactory.CreateResponse(remotingRequest, data);
        }
    }
}
