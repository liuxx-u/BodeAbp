using Abp;

namespace BodeAbp.Queue.Broker.Storage
{
    public class ChunkCompleteException : AbpException
    {
        public ChunkCompleteException(string message) : base(message) { }
    }
}
