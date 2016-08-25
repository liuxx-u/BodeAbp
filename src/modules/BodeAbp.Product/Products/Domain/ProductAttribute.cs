using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 商品属性关系
    /// </summary>
    [Table("Product#AttributeMap")]
    public class ProductAttribute : Entity<long>
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        /// <summary>
        /// 属性值Id
        /// </summary>
        public int AttributeOptionId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [ForeignKey("AttributeOptionId")]
        public virtual AttributeOption AttributeOption { get; set; }
    }
}
