using System.Collections.Generic;
using System.Reflection;
using Abp.EntityFramework.Migrations;

namespace Abp.EntityFramework.Default
{
    /// <summary>
    /// 默认数据库迁移配置类
    /// </summary>
    public class DefaultMigrationsConfigurationWithSeed : MigrationsConfigurationWithSeedBase<DefaultDbContext>
    {
        /// <summary>
        /// 实体映射程序集
        /// </summary>
        /// <returns></returns>
        public override ICollection<Assembly> GetMapperAssemblies()
        {
            return DefaultDbContextInitializer.Instance.MapperAssemblies;
        }
    }
}
