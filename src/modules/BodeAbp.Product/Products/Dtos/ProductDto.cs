using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Product.Products.Domain;

namespace BodeAbp.Product.Products.Dtos
{
    /// <summary>
    /// 商品Dto
    /// </summary>
    public abstract class ProductDto : EntityDto
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [StringLength(Domain.Product.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 商品简介
        /// </summary>
        [StringLength(Domain.Product.MaxBriefLength)]
        public string Brief { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 封面(16:9)
        /// </summary>
        public string Cover2 { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsOnShelf { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime? OnShelfTime { get; set; }

        /// <summary>
        /// 枚举测试
        /// </summary>
        public EnumTest EnumTest { get; set; }
    }

    [AutoMapTo(typeof(Domain.Product))]
    public class CreateProductInput : ProductDto, IInputDto
    {
    }

    [AutoMapTo(typeof(Domain.Product))]
    public class UpdateProductInput : ProductDto, IInputDto
    {
    }

    [AutoMapFrom(typeof(Domain.Product))]
    public class UpdateProductOutput : ProductDto, IOutputDto
    {
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
