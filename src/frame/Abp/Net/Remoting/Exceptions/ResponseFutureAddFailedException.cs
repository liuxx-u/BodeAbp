using Abp;
using System;

namespace Abp.Net.Remoting.Exceptions
{
    public class ResponseFutureAddFailedException : AbpException
    {
        public ResponseFutureAddFailedException(long requestSequence)
            : base(string.Format("Add remoting request response future failed. request sequence:{0}", requestSequence))
        {
        }
    }
}
