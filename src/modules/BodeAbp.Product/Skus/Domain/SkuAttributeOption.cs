using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using Abp;

namespace BodeAbp.Product.Skus.Domain
{
    /// <summary>
    /// SKU属性选项
    /// </summary>
    [Table("Product#SkuAttributeOption")]
    public class SkuAttributeOption : FullAuditedEntity
    {
        /// <summary>
        /// 选项值
        /// </summary>
        [Required]
        [StringLength(AbpStringLength.MaxLength32)]
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public int SkuAttributeId { get; set; }

        /// <summary>
        /// 属性模版
        /// </summary>
        [ForeignKey("SkuAttributeId")]
        public virtual SkuAttribute SkuAttribute { get; set; }
    }
}
