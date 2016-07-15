using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Forum.Plates.Domain
{
    /// <summary>
    /// 活动板块
    /// </summary>
    [Table("Forum#Plate")]
    public class Plate : FullAuditedEntity
    {
        #region 常量

        /// <summary>
        /// 板块名称<see cref="Name"/>最大长度
        /// </summary>
        public const int MaxNameLength = 32;

        #endregion

        #region 属性

        /// <summary>
        /// 板块名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 父级板块
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual Plate Parent { get; set; }

        #endregion
    }
}
