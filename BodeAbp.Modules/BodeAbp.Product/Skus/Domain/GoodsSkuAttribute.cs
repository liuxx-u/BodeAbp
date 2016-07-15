using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Skus.Domain
{
    /// <summary>
    /// Sku 属性关系
    /// </summary>
    [Table("Product#GoodsAttributeMap")]
    public class GoodsSkuAttribute : Entity<long>
    {
        /// <summary>
        /// 货品 Id
        /// </summary>
        public long GoodsId { get; set; }

        /// <summary>
        /// SKU属性值Id
        /// </summary>
        public int SkuAttributeOptionId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [ForeignKey("SkuAttributeOptionId")]
        public virtual SkuAttributeOption SkuAttributeOption { get; set; }
    }
}
