using Abp.Modules;
using System.Reflection;

namespace BodeAbp.Plugins.Core
{
    public class BodeAbpPluginsCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
