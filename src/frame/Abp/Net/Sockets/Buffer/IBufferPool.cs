namespace Abp.Net.Sockets.Buffer
{
    public interface IBufferPool : IPool<byte[]>
    {
        int BufferSize { get; }
    }
}
