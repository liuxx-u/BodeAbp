using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Broker.Client;
using System.Text;

namespace BodeAbp.Queue.Broker.RequestHandlers
{
    public class ProducerHeartbeatRequestHandler : IRequestHandler
    {
        private ProducerManager _producerManager;
        private ISerializer<byte[]> _binarySerializer;

        public ProducerHeartbeatRequestHandler(BrokerController brokerController)
        {
            _producerManager = IocManager.Instance.Resolve<ProducerManager>();
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            var producerId = Encoding.UTF8.GetString(remotingRequest.Body);
            _producerManager.RegisterProducer(producerId, context.Connection);
            return null;
        }
    }
}
