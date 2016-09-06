using Abp;
using Abp.Net.Remoting.Args;
using System;
using System.Net;

namespace Abp.Net.Remoting.Exceptions
{
    public class RemotingRequestException : AbpException
    {
        public RemotingRequestException(EndPoint serverEndPoint, RemotingRequest request, string errorMessage)
            : base(string.Format("Send request {0} to server [{1}] failed, errorMessage:{2}", request, serverEndPoint, errorMessage))
        {
        }
        public RemotingRequestException(EndPoint serverEndPoint, RemotingRequest request, Exception exception)
            : base(string.Format("Send request {0} to server [{1}] failed.", request, serverEndPoint), exception)
        {
        }
    }
}
