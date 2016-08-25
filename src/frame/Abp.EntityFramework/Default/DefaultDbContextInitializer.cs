using System;
using System.Data.Entity;
using Abp.EntityFramework.Initialize;

namespace Abp.EntityFramework.Default
{
    /// <summary>
    /// 默认 上下文初始化操作类
    /// </summary>
    public class DefaultDbContextInitializer : DbContextInitializerBase<DefaultDbContext>
    {
        private DefaultDbContextInitializer()
        {
            //添加迁移种子类
            CreateDatabaseInitializer = new DefaultCreateDbContextWithSeed();
            MigrateInitializer = new MigrateDatabaseToLatestVersion<DefaultDbContext, DefaultMigrationsConfigurationWithSeed>();
        }

        static DefaultDbContextInitializer()
        {
            _lazyInstance = new Lazy<DefaultDbContextInitializer>(() => new DefaultDbContextInitializer());
        }

        private static readonly Lazy<DefaultDbContextInitializer> _lazyInstance;

        /// <summary>
        /// 默认数据库初始化操作类的实例(单例)
        /// </summary>
        public static DefaultDbContextInitializer Instance
        {
            get { return _lazyInstance.Value; }
        }
    }
}