using System.Collections.Generic;
using System.Reflection;
using Abp.EntityFramework.Migrations;

namespace BodeAbp.Activity.DbContext
{
    /// <summary>
    /// 活动数据库迁移配置类
    /// </summary>
    public class ActivityMigrationsConfigurationWithSeed : MigrationsConfigurationWithSeedBase<ActivityDbContext>
    {
        /// <summary>
        /// 实体映射程序集
        /// </summary>
        /// <returns></returns>
        public override ICollection<Assembly> GetMapperAssemblies()
        {
            return ActivityDbContextInitializer.Instance.MapperAssemblies;
        }
    }
}
