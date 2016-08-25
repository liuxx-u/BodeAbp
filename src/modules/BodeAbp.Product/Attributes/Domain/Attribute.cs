using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Product.Attributes.Domain
{
    /// <summary>
    /// 属性模版
    /// </summary>
    [Table("Product#Attribute")]
    public class Attribute : FullAuditedEntity
    {
        /// <summary>
        /// 模版名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否必填项
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 是否在客户端展示
        /// </summary>
        public bool IsShowToClient { get; set; }

        /// <summary>
        /// 分类Id，null表示公共属性
        /// </summary>
        public int? ProductClassifyId { get; set; }

        /// <summary>
        /// 所在分类
        /// </summary>
        [ForeignKey("ProductClassifyId")]
        public virtual ProductClassify ProductClassify { get; set; }

        /// <summary>
        /// 属性值集合
        /// </summary>
        public virtual ICollection<AttributeOption> Options { get;set; }
    }
}
