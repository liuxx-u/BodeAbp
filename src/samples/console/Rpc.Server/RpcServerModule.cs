using Abp.EntityFramework;
using Abp.Modules;
using Abp.Rpc.Configuration;
using Abp.Rpc.Transport.DotNetty;
using BodeAbp.Activity;

namespace Rpc.Server
{
    [DependsOn(typeof(BodeAbpActivityModule),typeof(AbpEntityFrameworkModule))]
    public class RpcServerModule : AbpModule
    {
        public override void Initialize()
        {
            Configuration.Modules.AbpRpc()
                .InitRpcCore()
                .UseJsonCodec()
                .InitServerRuntime()
                .UseSharedFileRouteManager(@"d:\routes.txt")
                .UseDotNettyTransport();
        }
    }
}
