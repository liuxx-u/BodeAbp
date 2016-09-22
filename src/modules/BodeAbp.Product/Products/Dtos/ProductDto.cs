using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp;
using System.Collections.Generic;
using BodeAbp.Product.Attributes.Dtos;

namespace BodeAbp.Product.Products.Dtos
{
    /// <summary>
    /// 产品Dto
    /// </summary>
    public abstract class ProductDto : EntityDto
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        [StringLength(AbpStringLength.MaxLength64)]
        public string Name { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsOnShelf { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public int ClassifyId { get; set; }
    }
    
    /// <summary>
    /// 产品操作Dto
    /// </summary>
    [AutoMap(typeof(Domain.Product))]
    public class OperableProductDto : ProductDto, IDoubleWayDto
    {
        /// <summary>
        /// 产品属性
        /// </summary>
        public ICollection<OperableAttributeGroupDto> GroupAttributes { get; set; }

        /// <summary>
        /// 产品额外属性
        /// </summary>
        public ICollection<ProductExtendAttributeDto> ExtendAttributes { get; set; }

        /// <summary>
        /// 产品图片集合
        /// </summary>
        public ICollection<string> Albums { get; set; }
    }

    [AutoMapFrom(typeof(Domain.Product))]
    public class GetProductOutput : ProductDto, IOutputDto
    {
    }

    [AutoMapFrom(typeof(Domain.Product))]
    public class GetProductListOutput : ProductDto, IOutputDto
    {
    }
}
