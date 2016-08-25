using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Activity.Activities.Domain
{
    /// <summary>
    /// 活动类型
    /// </summary>
    [Table("Activity#Classify")]
    public class Classify : FullAuditedEntity
    {
        #region 常量

        /// <summary>
        /// 类型名称 <see cref="Name"/> 最大长度.
        /// </summary>
        public const int MaxNameLength = 32;

        #endregion

        #region 属性

        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 是否静态类型（静态类型不允许编辑和删除）
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        #endregion
    }
}
