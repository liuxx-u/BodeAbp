using Abp;

namespace EQueue.Broker.Exceptions
{
    public class BrokerCleanningException : AbpException
    {
        public BrokerCleanningException() : base("Broker is currently cleanning.") { }
    }
}
