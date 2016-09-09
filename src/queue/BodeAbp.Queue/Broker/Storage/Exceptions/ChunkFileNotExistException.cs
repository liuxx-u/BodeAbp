using Abp;

namespace BodeAbp.Queue.Broker.Storage
{
    public class ChunkFileNotExistException : AbpException
    {
        public ChunkFileNotExistException(string fileName) : base(string.Format("Chunk file '{0}' not exist.", fileName)) { }
    }
}
