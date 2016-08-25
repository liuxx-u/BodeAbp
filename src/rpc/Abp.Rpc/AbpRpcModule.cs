using Abp.Modules;
using Abp.Rpc.Configuration;

namespace Abp.Rpc
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpRpcModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Settings.Providers.Add<AbpRpcSettingProvider>();
            IocManager.Register<IAbpRpcModuleConfiguration, AbpRpcModuleConfiguration>();
        }
    }
}
