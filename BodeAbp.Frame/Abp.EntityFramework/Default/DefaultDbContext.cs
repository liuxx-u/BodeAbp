
namespace Abp.EntityFramework.Default
{
    /// <summary>
    /// 默认 EntityFramework 数据上下文
    /// </summary>
    public class DefaultDbContext : AbpDbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultDbContext() : base("Default") { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        protected DefaultDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }
}