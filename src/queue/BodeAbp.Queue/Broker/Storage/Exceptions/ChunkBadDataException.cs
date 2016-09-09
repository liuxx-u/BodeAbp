using Abp;

namespace BodeAbp.Queue.Broker.Storage
{
    public class ChunkBadDataException : AbpException
    {
        public ChunkBadDataException(string message) : base(message)
        {
        }
    }
}
