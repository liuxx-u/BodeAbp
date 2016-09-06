using Abp;
using Abp.Modules;
using BodeAbp.Queue.Configuration;

namespace BodeAbp.Queue
{
    [DependsOn(typeof(AbpKernelModule))]
    public class BodeAbpQueueModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IAbpQueueModuleConfiguration, AbpQueueModuleConfiguration>();
        }
    }
}
