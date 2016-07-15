using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;

namespace Abp.EntityFramework.Migrations
{
    /// <summary>
    /// 数据库存在时 种子迁移配置
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class MigrationsConfigurationWithSeedBase<TDbContext> : DbMigrationsConfiguration<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 初始化一个<see cref="MigrationsConfigurationWithSeedBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected MigrationsConfigurationWithSeedBase()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = typeof(TDbContext).FullName;

            SeedActions = new List<ISeedAction<TDbContext>>();
        }

        /// <summary>
        /// 获取 数据迁移初始化种子数据操作信息集合，各个模块可以添加自己的数据初始化操作
        /// </summary>
        public ICollection<ISeedAction<TDbContext>> SeedActions { get; private set; }

        /// <summary>
        /// 获取实体映射程序集
        /// </summary>
        public abstract ICollection<Assembly> GetMapperAssemblies();

        /// <summary>
        /// 获取迁移种子数据
        /// </summary>
        public void InitSeedActions()
        {
            var assemblies = GetMapperAssemblies();

            if (assemblies.Count == 0)
            {
                throw new InvalidOperationException(string.Format("上下文种子数据“{0}”初始化失败，实体映射程序集不存在", this.GetType().FullName));
            }
            Type baseType = typeof(ISeedAction<TDbContext>);
            Type[] seedTypes = assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && type != baseType && !type.IsAbstract).ToArray();
            SeedActions = seedTypes.Select(type => Activator.CreateInstance(type) as ISeedAction<TDbContext>).ToList();
        }

        /// <summary>
        /// 重写Seed方法
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(TDbContext context)
        {
            if (!SeedActions.Any())
            {
                InitSeedActions();
            }
            IEnumerable<ISeedAction<TDbContext>> seedActions = SeedActions.OrderBy(m => m.Order);
            foreach (ISeedAction<TDbContext> seedAction in seedActions)
            {
                seedAction.Action(context);
            }
        }
    }
}
