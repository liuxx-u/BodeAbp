using Abp.Dependency;
using Abp.Modules;
using Bode.Sms.Juhe;
using BodeAbp.Plugins.Core;

namespace BodeAbp.Sms.Juhe
{
    [DependsOn(typeof(BodeAbpPluginsCoreModule))]
    public class BodeAbpSmsJuheModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.Register<ISms, JuheSms>(DependencyLifeStyle.Transient);
        }
    }
}
