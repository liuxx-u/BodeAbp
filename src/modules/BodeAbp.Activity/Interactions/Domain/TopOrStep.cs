using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Activity.Interactions.Domain
{
    /// <summary>
    /// 顶或踩
    /// </summary>
    [Table("Activity#TopOrStep")]
    public class TopOrStep: CreationAuditedEntity<long>
    {
        /// <summary>
        /// 是否赞
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 评论Id
        /// </summary>
        public long CommentId { get; set; }
    }
}
