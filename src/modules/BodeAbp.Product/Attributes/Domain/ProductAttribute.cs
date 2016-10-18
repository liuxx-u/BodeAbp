using System.Collections.Generic;
using System.ComponentModel;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using Abp;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Product.Attributes.Domain
{
    /// <summary>
    /// 商品 属性
    /// </summary>
    [Table("Product_Attribute")]
    public class ProductAttribute : FullAuditedEntity
    {
        #region 属性

        /// <summary>
        /// 属性名称
        /// </summary>
        [Required]
        [StringLength(AbpStringLength.MaxLength32)]
        public string Name { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        [StringLength(AbpStringLength.MaxLength256)]
        public string Tips { get; set; }

        /// <summary>
        /// 验证规则(正则表达式)
        /// </summary>
        [StringLength(AbpStringLength.MaxLength64)]
        public string ValidateRegular { get; set; }

        /// <summary>
        /// 分组名
        /// </summary>
        [StringLength(AbpStringLength.MaxLength32)]
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
        public ProductAttributeType AttributeType { get; set; }

        /// <summary>
        /// 是否在客户端展示
        /// </summary>
        public bool IsShowToClient { get; set; }

        /// <summary>
        /// 分类Id，0表示公共属性
        /// </summary>
        public int ProductClassifyId { get; set; }

        /// <summary>
        /// 属性选项集合
        /// </summary>
        public virtual ICollection<ProductAttributeOption> AttributeOptions { get; set; }

        #endregion
    }

    /// <summary>
    /// 属性类型
    /// </summary>
    public enum ProductAttributeType
    {
        /// <summary>
        /// 普通文本
        /// </summary>
        [Description("普通文本")]
        Text = 1,

        /// <summary>
        /// 数字
        /// </summary>
        [Description("数字")]
        Number = 2,

        /// <summary>
        /// 价格
        /// </summary>
        [Description("价格")]
        Price = 3,

        /// <summary>
        /// 单项选择
        /// </summary>
        [Description("单项选择")]
        Switch = 4,

        /// <summary>
        /// 多项选择
        /// </summary>
        [Description("多项选择")]
        Multiple = 5,

        /// <summary>
        /// 日期
        /// </summary>
        [Description("日期")]
        DatePicker = 6,
        
        /// <summary>
        /// 文本域
        /// </summary>
        [Description("文本域")]
        TextArea = 7,

        /// <summary>
        /// 富文本
        /// </summary>
        [Description("富文本")]
        RichText = 8,

        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        Image = 9,

        /// <summary>
        /// 布尔值
        /// </summary>
        [Description("布尔值")]
         Boolean=10,

        /// <summary>
        /// 地区
        /// </summary>
        [Description("地区")]
        Area=11
    }
}
