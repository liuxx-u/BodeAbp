using System;
using Abp.AutoMapper;
using BodeAbp.Product.Attributes.Domain;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BodeAbp.Product.Attributes.Dtos
{
    public abstract class ProductClassifyDto : EntityDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int? ParentId { get; set; }

    }

	[AutoMapTo(typeof(ProductClassify))]
    public class CreateProductClassifyInput : ProductClassifyDto, IInputDto
    {
    }

	[AutoMapTo(typeof(ProductClassify))]
    public class UpdateProductClassifyInput : ProductClassifyDto, IInputDto
    {
    }

	[AutoMapFrom(typeof(ProductClassify))]
    public class UpdateProductClassifyOutput : ProductClassifyDto, IOutputDto
    {
    }

	[AutoMapFrom(typeof(ProductClassify))]
    public class GetProductClassifyOutput : ProductClassifyDto, IOutputDto
    {
    }

    [AutoMapFrom(typeof(ProductClassify))]
    public class GetProductClassifyListOutput : ProductClassifyDto, IOutputDto
    {
    }
}


