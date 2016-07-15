using System.Reflection;
using Abp.EntityFramework.Default;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using BodeAbp.Product.Providers;

namespace BodeAbp.Product
{
    /// <summary>
    /// 产品模块
    /// </summary>
    public class BodeAbpProductModule : AbpModule
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public const string CurrentVersion = "0.1.0";

        /// <summary>
        /// 初始化前执行
        /// </summary>
        public override void PreInitialize()
        {
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    BodeAbpProductConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "BodeAbp.Product.Localization.Source"
                        )
                    )
                );

            Configuration.Settings.Providers.Add<BodeAbpProductSettingProvider>();
            Configuration.Navigation.Providers.Add<BodeAbpProductNavigationProvider>();
            Configuration.Authorization.Providers.Add<BodeAbpProductAuthorizationProvider>();

            DefaultDbContextInitializer.Instance.MapperAssemblies.Add(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 初始化执行
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 初始化后执行
        /// </summary>
        public override void PostInitialize()
        {
            base.PostInitialize();
        }
    }
}
