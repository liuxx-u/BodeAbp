using System.Reflection;
using Abp;
using Abp.EntityFramework.Default;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using BodeAbp.Zero.Providers;

namespace BodeAbp.Zero
{
    [DependsOn(typeof(AbpKernelModule))]
    public class BodeAbpZeroModule : AbpModule
    {
        /// <summary>
        /// Current version of the zero module.
        /// </summary>
        public const string CurrentVersion = "0.1.0";

        public override void PreInitialize()
        {
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    BodeAbpZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "BodeAbp.Zero.Localization.Source"
                        )
                    )
                );
            
            Configuration.Authorization.Providers.Add<BodeAbpZeroAuthorizationProvider>();
            Configuration.Settings.Providers.Add<BodeAbpZeroSettingProvider>();

            DefaultDbContextInitializer.Instance.MapperAssemblies.Add(Assembly.GetExecutingAssembly());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
        }
    }
}
