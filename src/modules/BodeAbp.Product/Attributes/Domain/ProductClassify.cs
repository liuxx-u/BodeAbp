using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using Abp;

namespace BodeAbp.Product.Attributes.Domain
{
    /// <summary>
    /// 商品分类
    /// </summary>
    [Table("Product_Classify")]
    public class ProductClassify : FullAuditedEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(AbpStringLength.MaxLength32)]
        public string Name { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 父级Id字符串集合：1,2,3
        /// </summary>
        [StringLength(AbpStringLength.MaxLength64)]
        public string ParentIds { get; set; }

        /// <summary>
        /// 父级分类
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual ProductClassify Parent { get; set; }
    }
}
