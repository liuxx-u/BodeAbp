using Abp.Dependency;

namespace Abp.Rpc.Configuration
{
    public interface IAbpRpcModuleConfiguration
    {
        IIocManager IocManager { get; set; }

        /// <summary>
        /// 初始化RPC核心服务。
        /// </summary>
        IAbpRpcModuleConfiguration InitRpcCore();

        /// <summary>
        /// 使用Json编解码器。
        /// </summary>
        IAbpRpcModuleConfiguration UseJsonCodec();

        /// <summary>
        /// 初始化客户端运行时服务。
        /// </summary>
        IAbpRpcModuleConfiguration InitClientRuntime();

        /// <summary>
        /// 初始化服务端运行时服务。
        /// </summary>
        IAbpRpcModuleConfiguration InitServerRuntime();

        /// <summary>
        ///使用轮询的地址选择器。
        /// </summary>
        IAbpRpcModuleConfiguration UsePollingAddressSelector();

        /// <summary>
        ///使用随机的地址选择器。
        /// </summary>
        IAbpRpcModuleConfiguration UseRandomAddressSelector();

        /// <summary>
        ///使用共享文件路由管理者。
        /// </summary>
        IAbpRpcModuleConfiguration UseSharedFileRouteManager();
    }
}
