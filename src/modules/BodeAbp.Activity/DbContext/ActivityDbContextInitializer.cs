using System;
using System.Data.Entity;
using Abp.EntityFramework.Initialize;

namespace BodeAbp.Activity.DbContext
{
    /// <summary>
    /// 活动 上下文初始化操作类
    /// </summary>
    public class ActivityDbContextInitializer : DbContextInitializerBase<ActivityDbContext>
    {
        private ActivityDbContextInitializer()
        {
            //添加迁移种子类
            CreateDatabaseInitializer = new ActivityCreateDbContextWithSeed();
            MigrateInitializer = new MigrateDatabaseToLatestVersion<ActivityDbContext, ActivityMigrationsConfigurationWithSeed>();
        }

        static ActivityDbContextInitializer()
        {
            _lazyInstance = new Lazy<ActivityDbContextInitializer>(() => new ActivityDbContextInitializer());
        }

        private static readonly Lazy<ActivityDbContextInitializer> _lazyInstance;

        /// <summary>
        /// 默认数据库初始化操作类的实例(单例)
        /// </summary>
        public static ActivityDbContextInitializer Instance
        {
            get { return _lazyInstance.Value; }
        }
    }
}