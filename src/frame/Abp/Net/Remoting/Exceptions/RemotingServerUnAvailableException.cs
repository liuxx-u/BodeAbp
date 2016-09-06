using Abp;
using System;
using System.Net;

namespace Abp.Net.Remoting.Exceptions
{
    public class RemotingServerUnAvailableException : AbpException
    {
        public RemotingServerUnAvailableException(EndPoint serverEndPoint)
            : base(string.Format("Remoting server is unavailable, server address:{0}", serverEndPoint))
        {
        }
    }
}
