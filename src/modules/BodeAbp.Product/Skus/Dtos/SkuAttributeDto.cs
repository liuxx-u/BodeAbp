using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Product.Skus.Domain;

namespace BodeAbp.Product.Skus.Dtos
{
    /// <summary>
    /// SKU属性Dto
    /// </summary>
    [AutoMap(typeof(SkuAttribute))]
    public class SkuAttributeDto : EntityDto
    {
        /// <summary>
        /// SKU属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类Id，null表示公共属性
        /// </summary>
        public int? ProductClassifyId { get; set; }
    }
}
