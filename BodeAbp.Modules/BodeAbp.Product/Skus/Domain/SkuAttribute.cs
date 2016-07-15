using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Skus.Domain
{
    /// <summary>
    /// SKU属性模版
    /// </summary>
    [Table("Product#SkuAttribute")]
    public class SkuAttribute : FullAuditedEntity
    {
        /// <summary>
        /// SKU属性名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 分类Id，null表示公共属性
        /// </summary>
        public int? ProductClassifyId { get; set; }

        /// <summary>
        /// 所在分类
        /// </summary>
        [ForeignKey("ProductClassifyId")]
        public virtual ProductClassify ProductClassify { get; set; }

        /// <summary>
        /// 属性选项集合
        /// </summary>
        [ForeignKey("SkuAttributeId")]
        public virtual ICollection<SkuAttributeOption> Options { get; set; }
    }
}
