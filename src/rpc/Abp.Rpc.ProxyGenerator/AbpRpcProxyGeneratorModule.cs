using Abp.Dependency;
using Abp.Modules;
using Abp.Rpc.Client;
using Abp.Rpc.Client.Address;
using Abp.Rpc.Client.HealthChecks;
using Abp.Rpc.ProxyGenerator.Proxy;

namespace Abp.Rpc.ProxyGenerator
{
    [DependsOn(typeof(AbpRpcModule))]
    public class AbpRpcProxyGeneratorModule : AbpModule
    {
        public override void Initialize()
        {
        }
    }
}
