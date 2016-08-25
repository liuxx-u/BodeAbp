using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Activity.Interactions.Domain
{
    /// <summary>
    /// 活动评论回复
    /// </summary>
    [Table("Activity#Reply")]
    public class Reply : CreationAuditedEntity<long>
    {
        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 目标用户Id
        /// </summary>
        public long TargetUserId { get; set; }

        /// <summary>
        /// 评论Id
        /// </summary>
        public long CommentId { get; set; }
    }
}
