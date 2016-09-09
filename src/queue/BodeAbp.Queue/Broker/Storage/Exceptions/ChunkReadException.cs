using Abp;

namespace BodeAbp.Queue.Broker.Storage
{
    public class ChunkReadException : AbpException
    {
        public ChunkReadException(string message) : base(message) { }
    }
}
