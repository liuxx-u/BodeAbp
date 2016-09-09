using Abp.Extensions;
using System.IO;

namespace BodeAbp.Queue.Broker.Storage
{
    public class ChunkFooter
    {
        public const int Size = 128;
        public readonly int ChunkDataTotalSize;

        public ChunkFooter(int chunkDataTotalSize)
        {
            chunkDataTotalSize.CheckGreaterThan("chunkDataTotalSize", 0, true);
            ChunkDataTotalSize = chunkDataTotalSize;
        }

        public byte[] AsByteArray()
        {
            var array = new byte[Size];
            using (var stream = new MemoryStream(array))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(ChunkDataTotalSize);
                }
            }
            return array;
        }

        public static ChunkFooter FromStream(BinaryReader reader, Stream stream)
        {
            var chunkDataTotalSize = reader.ReadInt32();
            return new ChunkFooter(chunkDataTotalSize);
        }

        public override string ToString()
        {
            return string.Format("[ChunkDataTotalSize:{0}]", ChunkDataTotalSize);
        }
    }
}
