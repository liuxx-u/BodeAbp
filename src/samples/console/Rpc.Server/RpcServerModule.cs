using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using Abp.Modules;
using Abp.Rpc;
using Abp.Rpc.Configuration;
using Abp.Rpc.Transport.Simple;
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
                .UseSharedFileRouteManager()
                .UseSimpleTransport();
        }
    }
}
