using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Activity.Interactions.Domain;

namespace BodeAbp.Activity.Interactions.Dtos
{
    /// <summary>
    /// 评论Dto
    /// </summary>
    [AutoMapTo(typeof(Comment))]
    public class CreateCommentInput : IInputDto
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public long ActivityId { get; set; }
    }
}
