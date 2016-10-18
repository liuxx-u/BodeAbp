using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using Abp;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Product.Attributes.Domain
{
    /// <summary>
    /// 商品属性选项
    /// </summary>
    [Table("Product_AttributeOption")]
    public class ProductAttributeOption : FullAuditedEntity
    {
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [StringLength(AbpStringLength.MaxLength32)]
        public string Value { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(AbpStringLength.MaxLength256)]
        public string Remark { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// 属性模版
        /// </summary>
        public virtual ProductAttribute Attribute { get; set; }
    }
}
