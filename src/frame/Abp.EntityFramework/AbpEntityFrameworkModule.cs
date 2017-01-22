using System.Reflection;
using Abp.EntityFramework.Default;
using Abp.EntityFramework.Uow;
using Abp.Modules;
using Abp.Reflection;
using Castle.Core.Logging;
using Abp.Configuration.Startup;
using Castle.MicroKernel.Registration;
using Abp.Domain.Uow;
using Abp.Configuration;
using Abp.Dependency;

namespace Abp.EntityFramework
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in EntityFramework.
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpEntityFrameworkModule : AbpModule
    {
        public ILogger Logger { get; set; }

        private readonly ITypeFinder _typeFinder;

        public AbpEntityFrameworkModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
            Logger = NullLogger.Instance;
        }

        public override void PreInitialize()
        {
            Configuration.ReplaceService<IUnitOfWorkFilterExecuter>(() =>
            {
                IocManager.IocContainer.Register(
                    Component
                    .For<IUnitOfWorkFilterExecuter, IEfUnitOfWorkFilterExecuter>()
                    .ImplementedBy<EfDynamicFiltersUnitOfWorkFilterExecuter>()
                    .LifestyleTransient()
                );
            });

            IocManager.Register<IAbpEfModuleConfiguration, AbpEfModuleConfiguration>(DependencyLifeStyle.Singleton);

            //添加默认数据库初始化类
            DbContextManager.Instance.RegisterInitializer(typeof(DefaultDbContext), DefaultDbContextInitializer.Instance);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component.For(typeof(IDbContextProvider<>))
                    .ImplementedBy(typeof(UnitOfWorkDbContextProvider<>))
                    .LifestyleTransient()
                );
            
            DbContextManager.Instance.Initialize();
        }
    }
}
