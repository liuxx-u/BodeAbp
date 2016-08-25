using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Modules;
using Abp.Rpc;
using Abp.Rpc.Configuration;
using Abp.Rpc.Transport.Simple;

namespace Rpc.Server
{
    [DependsOn(typeof(AbpRpcModule))]
    public class RpcServerModule : AbpModule
    {
        public override void Initialize()
        {
            Configuration.Modules.AbpRpc()
                .InitRpcCore()
                .UseJsonCodec()
                .InitServerRuntime()
                .UseSharedFileRouteManager()
                .UseSimpleTransport();
        }
    }
}
