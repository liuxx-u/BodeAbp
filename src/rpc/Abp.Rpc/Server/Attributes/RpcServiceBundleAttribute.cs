using System;

namespace Abp.Rpc.Server.Attributes
{
    /// <summary>
    /// 服务集标记。
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RpcServiceBundleAttribute : Attribute
    {
    }
}
