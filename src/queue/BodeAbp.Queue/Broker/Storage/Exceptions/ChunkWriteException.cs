using Abp;

namespace BodeAbp.Queue.Broker.Storage
{
    public class ChunkWriteException : AbpException
    {
        public ChunkWriteException(string chunkName, string message) : base(string.Format("{0} write failed, message: {1}", chunkName, message)) { }
    }
}
