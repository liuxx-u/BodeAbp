using Abp;

namespace BodeAbp.Queue.Broker.Storage
{
    public class ChunkCreateException : AbpException
    {
        public ChunkCreateException(string message) : base(message) { }
    }
}
