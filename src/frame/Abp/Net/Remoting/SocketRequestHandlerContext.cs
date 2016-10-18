using Abp.Net.Remoting.Args;
using Abp.Net.Sockets;
using System;

namespace Abp.Net.Remoting
{
    public class SocketRequestHandlerContext : IRequestHandlerContext
    {
        public ITcpConnection Connection { get; private set; }
        public Action<RemotingResponse> SendRemotingResponse { get; private set; }

        public SocketRequestHandlerContext(ITcpConnection connection, Action<byte[]> sendReplyAction)
        {
            Connection = connection;
            SendRemotingResponse = remotingResponse =>
            {
                sendReplyAction(RemotingUtils.BuildResponseMessage(remotingResponse));
            };
        }
    }
}
