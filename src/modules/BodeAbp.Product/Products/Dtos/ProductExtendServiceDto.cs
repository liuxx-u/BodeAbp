using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Product.Products.Domain;

namespace BodeAbp.Product.Products.Dtos
{
    /// <summary>
    /// 产品额外属性Dto
    /// </summary>
    [AutoMap(typeof(ProductExtendService))]
    public class ProductExtendServiceDto : EntityDto<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 是否按天计费
        /// </summary>
        public bool BillingByDay { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }
    }
}
