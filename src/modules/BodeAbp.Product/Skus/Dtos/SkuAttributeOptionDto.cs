using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Product.Skus.Domain;

namespace BodeAbp.Product.Skus.Dtos
{
    /// <summary>
    /// SKU属性选项Dto
    /// </summary>
    [AutoMap(typeof(SkuAttributeOption))]
    public class SkuAttributeOptionDto : EntityDto, IDoubleWayDto
    {
        /// <summary>
        /// 选项值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public int SkuAttributeId { get; set; }
    }
}
