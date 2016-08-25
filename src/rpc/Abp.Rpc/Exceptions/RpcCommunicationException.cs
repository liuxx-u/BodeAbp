using System;

namespace Abp.Rpc.Exceptions
{
    /// <summary>
    /// RPC通讯异常（与服务端进行通讯时发生的异常）。
    /// </summary>
    public class RpcCommunicationException : AbpException
    {
        /// <summary>
        /// 初始化一个新的Rpc异常实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="innerException">内部异常。</param>
        public RpcCommunicationException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}
