using System;
using Abp.AutoMapper;
using BodeAbp.Product.Attributes.Domain;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BodeAbp.Product.Attributes.Dtos
{
    public abstract class AttributeOptionDto : EntityDto
    {
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public int AttributeId { get; set; }

    }

	[AutoMapTo(typeof(AttributeOption))]
    public class CreateAttributeOptionInput : AttributeOptionDto, IInputDto
    {
    }

	[AutoMapTo(typeof(AttributeOption))]
    public class UpdateAttributeOptionInput : AttributeOptionDto, IInputDto
    {
    }

	[AutoMapFrom(typeof(AttributeOption))]
    public class UpdateAttributeOptionOutput : AttributeOptionDto, IOutputDto
    {
    }

	[AutoMapFrom(typeof(AttributeOption))]
    public class GetAttributeOptionOutput : AttributeOptionDto, IOutputDto
    {
    }

    [AutoMapFrom(typeof(AttributeOption))]
    public class GetAttributeOptionListOutput : AttributeOptionDto, IOutputDto
    {
    }
}


