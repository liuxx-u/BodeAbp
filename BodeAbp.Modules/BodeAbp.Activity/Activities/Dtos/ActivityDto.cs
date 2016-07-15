using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BodeAbp.Activity.Activities.Dtos
{
    public abstract class ActivityDto : EntityDto
    {
        /// <summary>
        /// 活动标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        [Required]
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
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublish { get; set; }

        /// <summary>
        /// 活动类型Id
        /// </summary>
        public long? ClassifyId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

    }

	[AutoMapTo(typeof(Domain.Activity))]
    public class CreateActivityInput : ActivityDto, IInputDto
    {
    }

	[AutoMapTo(typeof(Domain.Activity))]
    public class UpdateActivityInput : ActivityDto, IInputDto
    {
    }

	[AutoMapFrom(typeof(Domain.Activity))]
    public class UpdateActivityOutput : ActivityDto, IOutputDto
    {
    }

	[AutoMapFrom(typeof(Domain.Activity))]
    public class GetActivityOutput : ActivityDto, IOutputDto
    {
    }

    [AutoMapFrom(typeof(Domain.Activity))]
    public class GetActivityListOutput : ActivityDto, IOutputDto
    {
    }
}


