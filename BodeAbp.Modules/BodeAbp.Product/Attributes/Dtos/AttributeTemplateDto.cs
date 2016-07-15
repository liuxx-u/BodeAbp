using Abp.AutoMapper;
using BodeAbp.Product.Attributes.Domain;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BodeAbp.Product.Attributes.Dtos
{
    public abstract class AttributeDto : EntityDto
    {
        /// <summary>
        /// 模版名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 是否必填项
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 是否SKU属性
        /// </summary>
        public bool IsSkuAttribute { get; set; }

        /// <summary>
        /// 是否在客户端展示
        /// </summary>
        public bool IsShowToClient { get; set; }

    }

	[AutoMapTo(typeof(Attribute))]
    public class CreateAttributeInput : AttributeDto, IInputDto
    {
    }

	[AutoMapTo(typeof(Attribute))]
    public class UpdateAttributeInput : AttributeDto, IInputDto
    {
    }

	[AutoMapFrom(typeof(Attribute))]
    public class UpdateAttributeOutput : AttributeDto, IOutputDto
    {
    }

	[AutoMapFrom(typeof(Attribute))]
    public class GetAttributeOutput : AttributeDto, IOutputDto
    {
    }

    [AutoMapFrom(typeof(Attribute))]
    public class GetAttributeListOutput : AttributeDto, IOutputDto
    {
    }
}


