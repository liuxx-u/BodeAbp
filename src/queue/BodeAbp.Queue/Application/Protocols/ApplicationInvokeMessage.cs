using System;

namespace BodeAbp.Queue.Application.Protocols
{
    /// <summary>
    /// 应用服务执行Messahe
    /// </summary>
    [Serializable]
    public class ApplicationInvokeMessage
    {
        /// <summary>
        /// 执行的类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 执行的方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 调用的参数集合
        /// </summary>
        public object[] Args { get; set; }
    }
}
