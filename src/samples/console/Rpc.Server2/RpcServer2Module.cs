using Abp.EntityFramework;
using Abp.Modules;
using Abp.Rpc.Configuration;
using Abp.Rpc.Transport.DotNetty;
using BodeAbp.Product;

namespace Rpc.Server2
{
    [DependsOn(typeof(BodeAbpProductModule), typeof(AbpEntityFrameworkModule))]
    public class RpcServerModule : AbpModule
    {
        public override void Initialize()
        {
            Configuration.Modules.AbpRpc()
                .InitRpcCore()
                .UseJsonCodec()
                .InitServerRuntime()
                .UseSharedFileRouteManager(@"d:\routes2.txt")
                .UseDotNettyTransport();
        }
    }
}
