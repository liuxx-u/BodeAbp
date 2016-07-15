using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Zero.Permissions.Domain
{
    /// <summary>
    /// 权限基类
    /// </summary>
    public abstract class PermissionBase : CreationAuditedEntity<long>
    {
        /// <summary>
        /// 权限名称 <see cref="Name"/> 最大长度.
        /// </summary>
        public const int MaxNameLength = 128;
        
        /// <summary>
        /// 权限名称
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否授权
        /// Default value: true.
        /// </summary>
        public virtual bool IsGranted { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected PermissionBase()
        {
            IsGranted = true;
        }
    }
}
