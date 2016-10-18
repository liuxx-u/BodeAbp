using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp;
using System.Collections.Generic;
using BodeAbp.Product.Attributes.Dtos;
using System;
using BodeAbp.Product.Products.Domain;

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
        public decimal OriginPrice { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Inventory { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsOnShelf { get; set; }
        
        /// <summary>
        /// 所在地区 Id
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public int ClassifyId { get; set; }
    }
    
    /// <summary>
    /// 产品操作Dto
    /// </summary>
    [AutoMap(typeof(Domain.Product))]
    public class OperableProductDto : ProductDto
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperableProductDto()
        {
            GroupAttributes = new List<OperableAttributeGroupDto>();
            Albums = new List<string>();
        }

        /// <summary>
        /// 产品属性
        /// </summary>
        public ICollection<OperableAttributeGroupDto> GroupAttributes { get; set; }

        /// <summary>
        /// 产品图片集合
        /// </summary>
        public ICollection<string> Albums { get; set; }
    }

    [AutoMapFrom(typeof(Domain.Product))]
    public class GetProductOutput : ProductDto
    {
    }

    [AutoMapFrom(typeof(Domain.Product))]
    public class GetProductListOutput : ProductDto
    {

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime? OnShelfTime { get; set; }

        /// <summary>
        /// 下架时间
        /// </summary>
        public DateTime? OffShelfTime { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesNo { get; set; }
    }
}
