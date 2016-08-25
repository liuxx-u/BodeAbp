using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Activity.Interactions.Domain;

namespace BodeAbp.Activity.Interactions.Dtos
{
    /// <summary>
    /// 回复Dto
    /// </summary>
    [AutoMapTo(typeof(Reply))]
    public class CreateReplyInput : IInputDto
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
