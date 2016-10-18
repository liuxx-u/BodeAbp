using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using BodeAbp.Activity.Interactions.Domain;
using BodeAbp.Activity.Participants.Domain;

namespace BodeAbp.Activity.Activities.Domain
{
    /// <summary>
    /// 活动
    /// </summary>
    [Table("Activity#Activity")]
    public class Activity : FullAuditedEntity<long>
    {
        #region 常量

        /// <summary>
        /// 活动标题<see cref="Title"/>最大长度
        /// </summary>
        public const int MaxTitleLength = 128;

        /// <summary>
        /// 活动描述<see cref="Description"/>最大长度
        /// </summary>
        public const int MaxDescriptionLength = 256;

        #endregion
        
        #region 属性
        
        /// <summary>
        /// 活动标题
        /// </summary>
        [Required]
        [StringLength(MaxTitleLength)]
        public string Title { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        [Required]
        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// 活动内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 活动封面图
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 最大参与人数
        /// </summary>
        public long Limit { get; set; }

        /// <summary>
        /// 是否免费
        /// </summary>
        public bool IsFree { get; set; }

        /// <summary>
        /// 报名费
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublish { get; set; }

        /// <summary>
        /// 热度
        /// </summary>
        public long HotNo { get; set; }

        /// <summary>
        /// 浏览数
        /// </summary>
        public long BrowseNo { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public long CommentNo { get; set; }

        /// <summary>
        /// 参与人数
        /// </summary>
        public int ParticipantNo { get; set; }

        /// <summary>
        /// 活动类型Id
        /// </summary>
        public int? ClassifyId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        [ForeignKey("ClassifyId")]
        public virtual Classify Classify { get; set; }

        /// <summary>
        /// 资源集合
        /// </summary>
        [ForeignKey("ActivityId")]
        public virtual ICollection<Asset> Assets { get; set; }

        /// <summary>
        /// 评论集合
        /// </summary>
        [ForeignKey("ActivityId")]
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// 参与者集合
        /// </summary>
        [ForeignKey("ActivityId")]
        public virtual ICollection<Paticipant> Paticipants { get; set; }

        #endregion
    }
}
