using Abp.Rpc.Configuration;
using Abp.Rpc.Server;

namespace Abp.Rpc.Transport.DotNetty
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 使用Simple进行传输。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IAbpRpcModuleConfiguration UseDotNettyTransport(this IAbpRpcModuleConfiguration config)
        {
            config.IocManager.Register<ITransportClientFactory, DotNettyTransportClientFactory>();
            config.IocManager.Register<IMessageListener, DotNettyServerMessageListener>();
            config.IocManager.Register<IServiceHost, DefaultServiceHost>();

            return config;
        }
    }
}
