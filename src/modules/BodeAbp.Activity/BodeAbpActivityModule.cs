using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using BodeAbp.Activity.Providers;
using BodeAbp.Activity.DbContext;
using Abp.EntityFramework;

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

            //Configuration.Settings.Providers.Add<BodeAbpActivitySettingProvider>();
            Configuration.Authorization.Providers.Add<BodeAbpActivityAuthorizationProvider>();

            //添加活动数据库初始化类
            ActivityDbContextInitializer.Instance.MapperAssemblies.Add(Assembly.GetExecutingAssembly());
            DbContextManager.Instance.RegisterInitializer(typeof(ActivityDbContext), ActivityDbContextInitializer.Instance);
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
