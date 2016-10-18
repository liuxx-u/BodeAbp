using Abp.EntityFramework;
using Abp.EntityFramework.Default;
using Abp.Modules;
using System.Reflection;

namespace BodeAbp.Product
{
    [DependsOn(typeof(AbpEntityFrameworkModule))]
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
            //Configuration.Settings.Providers.Add<BodeAbpProductSettingProvider>();
            //Configuration.Authorization.Providers.Add<BodeAbpProductAuthorizationProvider>();

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
