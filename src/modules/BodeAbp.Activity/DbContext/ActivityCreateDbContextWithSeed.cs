using Abp.EntityFramework.Migrations;
using System.Collections.Generic;
using System.Reflection;

namespace BodeAbp.Activity.DbContext
{
    /// <summary>
    /// 活动数据库初始化种子
    /// </summary>
    public class ActivityCreateDbContextWithSeed : CreateDatabaseIfNotExistsWithSeedBase<ActivityDbContext>
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
