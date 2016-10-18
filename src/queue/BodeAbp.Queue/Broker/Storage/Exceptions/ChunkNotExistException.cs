using Abp;

namespace BodeAbp.Queue.Broker.Storage
{
    public class ChunkNotExistException : AbpException
    {
        public ChunkNotExistException(long position, int chunkNum) : base(string.Format("Chunk not exist, position: {0}, chunkNum: {1}", position, chunkNum)) { }
    }
}
