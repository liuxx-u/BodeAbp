using Abp.EntityFramework;
using BodeAbp.Zero.Auditing.Domain;

namespace BodeAbp.Zero.EntityFramework.ModelConfigs
{
    /// <summary>
    /// 审计日志模型配置类
    /// </summary>
    public class AuditLogConfiguration : EntityConfigurationBase<AuditLog, long>
    {
    }
}
