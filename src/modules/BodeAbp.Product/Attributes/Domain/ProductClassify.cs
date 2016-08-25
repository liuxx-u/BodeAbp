using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Product.Attributes.Domain
{
    /// <summary>
    /// 分类
    /// </summary>
    [Table("Product#Classify")]
    public class ProductClassify : FullAuditedEntity
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

        /// <summary>
        /// 父级Id字符串集合：1,2,3
        /// </summary>
        public string ParentIds { get; set; }

        /// <summary>
        /// 父级分类
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual ProductClassify Parent { get; set; }
    }
}
