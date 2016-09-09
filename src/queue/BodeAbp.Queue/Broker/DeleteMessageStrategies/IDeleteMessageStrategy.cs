using BodeAbp.Queue.Broker.Storage;
using System.Collections.Generic;

namespace BodeAbp.Queue.Broker.DeleteMessageStrategies
{
    public interface IDeleteMessageStrategy
    {
        IEnumerable<Chunk> GetAllowDeleteChunks(ChunkManager chunkManager, long maxMessagePosition);
    }
}
