using System;
using Abp.AutoMapper;
using BodeAbp.Product.Attributes.Domain;
using Abp.Application.Services.Dto;

namespace BodeAbp.Product.Attributes.Dtos
{
    /// <summary>
    /// 属性选项Dto
    /// </summary>
    public class AttributeOptionDto : EntityDto
    {
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public int AttributeId { get; set; }
    }
}


