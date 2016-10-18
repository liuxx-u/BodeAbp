using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 商品属性关系
    /// </summary>
    [Table("Product_ProductAttribute")]
    public class ProductAttributeMap : Entity<long>
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
        /// 属性Id
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        [ForeignKey("AttributeId")]
        public virtual ProductAttribute Attribute { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 属性选项Id集合（当属性类型为Switch/Multiple时有效）
        /// </summary>
        public string AttributeOptionIds { get; set; }
    }
}
