using Abp.AutoMapper;
using BodeAbp.Product.Products.Domain;

namespace BodeAbp.Product.Products.Dtos
{
    /// <summary>
    /// 产品额外属性Dto
    /// </summary>
    [AutoMap(typeof(ProductExtendAttribute))]
    public class ProductExtendAttributeDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
