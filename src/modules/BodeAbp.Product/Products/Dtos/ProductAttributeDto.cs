using Abp.AutoMapper;
using BodeAbp.Product.Products.Domain;

namespace BodeAbp.Product.Products.Dtos
{
    /// <summary>
    /// 产品属性Dto
    /// </summary>
    [AutoMap(typeof(ProductAttribute))]
    public class ProductAttributeDto
    {
        /// <summary>
        /// 属性Id
        /// </summary>
        public int AttributeId { get; set; }
        
        /// <summary>
        /// 属性选项Id
        /// </summary>
        public int? AttributeOptionId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}
