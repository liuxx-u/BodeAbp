namespace Abp.Configuration
{
    public interface IAbpEfModuleConfiguration
    {
        /// <summary>
        /// 是否自动迁移数据库，默认：false
        /// </summary>
        bool AutoMigrateDatabase { get; set; }
    }
}
