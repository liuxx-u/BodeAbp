namespace BodeAbp.Queue.Broker.Storage
{
    public interface IFileNamingStrategy
    {
        string GetFileNameFor(string path, int index);
        string[] GetChunkFiles(string path);
        string[] GetTempFiles(string path);
    }
}
