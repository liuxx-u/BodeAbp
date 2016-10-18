using Abp.Net.Remoting.Args;
using Abp.Net.Sockets;
using System;

namespace Abp.Net.Remoting
{
    public interface IRequestHandlerContext
    {
        ITcpConnection Connection { get; }
        Action<RemotingResponse> SendRemotingResponse { get; }
    }
}
