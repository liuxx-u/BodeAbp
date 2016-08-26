using Abp.Rpc.Configuration;
using Abp.Rpc.ProxyGenerator.Proxy;

namespace Abp.Rpc.ProxyGenerator
{
    public static class ServiceCollectionExtensions
    {
        public static IAbpRpcModuleConfiguration AddClient(this IAbpRpcModuleConfiguration config)
        {
            config.InitRpcCore()
                .UseJsonCodec()
                .UsePollingAddressSelector()
                .InitClientRuntime()
                .InitClientProxy();
            return config;
        }

        public static IAbpRpcModuleConfiguration InitClientProxy(this IAbpRpcModuleConfiguration config)
        {
            config.IocManager.Register<IServiceProxyGenerater, ServiceProxyGenerater>();
            config.IocManager.Register<IServiceProxyFactory, ServiceProxyFactory>();
            return config;
        }
    }
}
