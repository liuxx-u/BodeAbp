using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Product.Attributes.Domain
{
    /// <summary>
    /// 属性
    /// </summary>
    [Table("Product#Attribute")]
    public class Attribute : FullAuditedEntity
    {
        /// <summary>
        /// 属性名称
        /// </summary>
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
        /// 属性类型
        /// </summary>
        public AttributeType AttributeType { get; set; }

        /// <summary>
        /// 是否在客户端展示
        /// </summary>
        public bool IsShowToClient { get; set; }

        /// <summary>
        /// 分类Id，0表示公共属性
        /// </summary>
        public int ProductClassifyId { get; set; }

        /// <summary>
        /// 属性值集合
        /// </summary>
        public virtual ICollection<AttributeOption> Options { get;set; }
    }

    /// <summary>
    /// 属性类型
    /// </summary>
    public enum AttributeType
    {
        [Description("普通文本")]
        Text = 1,

        [Description("数字")]
        Number = 2,

        [Description("价格")]
        Price = 3,

        [Description("单项选择")]
        Switch = 4,

        [Description("多项选择")]
        DropDown = 5,

        [Description("富文本")]
        RichText = 6,

        [Description("图片")]
        Image = 7,

        [Description("布尔值")]
         Boolean=8
    }
}
