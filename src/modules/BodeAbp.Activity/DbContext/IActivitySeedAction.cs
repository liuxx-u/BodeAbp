using Abp.EntityFramework.Migrations;

namespace BodeAbp.Activity.DbContext
{
    /// <summary>
    /// 活动数据库种子迁移接口
    /// </summary>
    public interface IActivitySeedAction : ISeedAction<ActivityDbContext>
    {
    }
}
