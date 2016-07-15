using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Activity.Interactions.Domain
{
    /// <summary>
    /// 活动评论
    /// </summary>
    [Table("Activity#Comment")]
    public class Comment : FullAuditedEntity<long>
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public long ActivityId { get; set; }

        /// <summary>
        /// 回复数
        /// </summary>
        public long ReplyNo { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public long TopNo { get; set; }

        /// <summary>
        /// 踩 数
        /// </summary>
        public long StepNo { get; set; }

        /// <summary>
        /// 回复集合
        /// </summary>
        [ForeignKey("CommentId")]
        public virtual ICollection<Reply> Replies { get; set; }
        
        /// <summary>
        /// 顶/踩 集合
        /// </summary>
        [ForeignKey("CommentId")]
        public virtual ICollection<TopOrStep> TopOrSteps { get; set; }
    }
}
