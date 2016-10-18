using System.Data.Entity;


namespace Abp.EntityFramework.Migrations
{
    /// <summary>
    /// 初始化种子数据接口
    /// </summary>
    public interface ISeedAction<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 获取 操作排序，数值越小越先执行
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 定义种子数据初始化过程
        /// </summary>
        /// <param name="context">数据上下文</param>
        void Action(DbContext context);
    }
}