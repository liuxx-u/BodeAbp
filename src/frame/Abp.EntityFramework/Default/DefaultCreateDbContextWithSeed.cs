using System.Collections.Generic;
using System.Reflection;
using Abp.EntityFramework.Migrations;

namespace Abp.EntityFramework.Default
{
    /// <summary>
    /// 默认数据库初始化种子
    /// </summary>
    public class DefaultCreateDbContextWithSeed : CreateDatabaseIfNotExistsWithSeedBase<DefaultDbContext>
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
