using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Product.Attributes.Domain
{
    /// <summary>
    /// 属性选项
    /// </summary>
    [Table("Product#AttributeOption")]
    public class AttributeOption : FullAuditedEntity
    {
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// 属性模版
        /// </summary>
        public virtual Attribute Attribute { get; set; }
    }
}
