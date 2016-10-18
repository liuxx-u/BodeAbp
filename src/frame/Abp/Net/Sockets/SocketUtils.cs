using Abp.Helper;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Abp.Net.Sockets
{
    public class SocketUtils
    {
        public static IPAddress GetLocalIPV4()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
        }
        public static Socket CreateSocket(int sendBufferSize, int receiveBufferSize)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            socket.Blocking = false;
            socket.SendBufferSize = sendBufferSize;
            socket.ReceiveBufferSize = receiveBufferSize;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            return socket;
        }
        public static void ShutdownSocket(Socket socket)
        {
            if (socket == null) return;
            ActionHelper.EatException(() => socket.Shutdown(SocketShutdown.Both));
            ActionHelper.EatException(() => socket.Close(10000));
        }
        public static void CloseSocket(Socket socket)
        {
            if (socket == null) return;

            ActionHelper.EatException(() => socket.Close(10000));
        }
    }
}
