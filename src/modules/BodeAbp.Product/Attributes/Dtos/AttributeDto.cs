using Abp.AutoMapper;
using BodeAbp.Product.Attributes.Domain;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BodeAbp.Product.Attributes.Dtos
{
    /// <summary>
    /// 属性Dto
    /// </summary>
    [AutoMap(typeof(Attribute))]
    public class AttributeDto : EntityDto, IDoubleWayDto
    {
        /// <summary>
        /// 模版名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 验证规则(正则表达式)
        /// </summary>
        public string ValidateRegular { get; set; }

        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 是否必填项
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public AttributeType AttributeType { get; set; }

        /// <summary>
        /// 是否在客户端展示
        /// </summary>
        public bool IsShowToClient { get; set; }

        /// <summary>
        /// 分类Id，null表示公共属性
        /// </summary>
        public int? ProductClassifyId { get; set; }
    }
}


