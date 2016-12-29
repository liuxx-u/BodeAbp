
using Abp.EntityFramework;

namespace BodeAbp.Activity.DbContext
{
    /// <summary>
    /// 活动 EntityFramework 数据上下文
    /// </summary>
    public class ActivityDbContext : AbpDbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ActivityDbContext() : base("Activity") { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        protected ActivityDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }
}