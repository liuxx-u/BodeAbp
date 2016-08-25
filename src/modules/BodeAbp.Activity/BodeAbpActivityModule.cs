using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework.Default;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using BodeAbp.Activity.Providers;

namespace BodeAbp.Activity
{
    public class BodeAbpActivityModule : AbpModule
    {
        /// <summary>
        /// Current version of the zero module.
        /// </summary>
        public const string CurrentVersion = "0.1.0";

        public override void PreInitialize()
        {
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    BodeAbpActivityConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "BodeAbp.Activity.Localization.Source"
                        )
                    )
                );

            //Configuration.Settings.Providers.Add<BodeAbpZeroSettingProvider>();
            Configuration.Navigation.Providers.Add<BodeAbpActivityNavigationProvider>();
            Configuration.Authorization.Providers.Add<BodeAbpActivityAuthorizationProvider>();

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
