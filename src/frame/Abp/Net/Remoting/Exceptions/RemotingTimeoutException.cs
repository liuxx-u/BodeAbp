using Abp;
using Abp.Net.Remoting.Args;
using System;
using System.Net;

namespace Abp.Net.Remoting.Exceptions
{
    public class RemotingTimeoutException : AbpException
    {
        public RemotingTimeoutException(EndPoint serverEndPoint, RemotingRequest request, long timeoutMillis)
            : base(string.Format("Wait response from server[{0}] timeout, request:{1}, timeoutMillis:{2}ms", serverEndPoint, request, timeoutMillis))
        {
        }
    }
}
