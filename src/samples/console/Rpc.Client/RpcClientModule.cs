using Abp.Modules;
using Abp.Rpc;
using Abp.Rpc.Configuration;
using Abp.Rpc.ProxyGenerator;
using Abp.Rpc.Transport.DotNetty;

namespace Rpc.Client
{
    [DependsOn(typeof(AbpRpcProxyGeneratorModule), typeof(AbpRpcModule))]
    public class RpcClientModule : AbpModule
    {
        public override void Initialize()
        {
            Configuration.Modules.AbpRpc()
                .AddClient()
                .UseSharedFileRouteManager()
                .UseDotNettyTransport();
        }
    }
}
