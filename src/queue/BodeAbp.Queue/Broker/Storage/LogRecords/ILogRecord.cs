using System.IO;

namespace BodeAbp.Queue.Broker.Storage.LogRecords
{
    public interface ILogRecord
    {
        void WriteTo(long logPosition, BinaryWriter writer);
        void ReadFrom(byte[] recordBuffer);
    }
}
