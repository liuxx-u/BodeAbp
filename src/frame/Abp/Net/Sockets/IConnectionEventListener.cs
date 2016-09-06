using System.Net.Sockets;

namespace Abp.Net.Sockets
{
    public interface IConnectionEventListener
    {
        void OnConnectionAccepted(ITcpConnection connection);
        void OnConnectionEstablished(ITcpConnection connection);
        void OnConnectionFailed(SocketError socketError);
        void OnConnectionClosed(ITcpConnection connection, SocketError socketError);
    }
}
