using Abp;
using Abp.Dependency;
using Abp.Modules;
using Abp.Plugins.SMS;
using Bode.Sms.Juhe;

namespace BodeAbp.Sms.Juhe
{
    [DependsOn(typeof(AbpKernelModule))]
    public class BodeAbpSmsJuheModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.Register<ISms, JuheSms>(DependencyLifeStyle.Transient);
        }
    }
}
