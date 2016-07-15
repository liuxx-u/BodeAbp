using System;
using Abp.AutoMapper;
using BodeAbp.Activity.Activities.Domain;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BodeAbp.Activity.Activities.Dtos
{
    public abstract class ClassifyDto : EntityDto
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 是否静态类型（静态类型不允许编辑和删除）
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

    }

	[AutoMapTo(typeof(Classify))]
    public class CreateClassifyInput : ClassifyDto, IInputDto
    {
    }

	[AutoMapTo(typeof(Classify))]
    public class UpdateClassifyInput : ClassifyDto, IInputDto
    {
    }

	[AutoMapFrom(typeof(Classify))]
    public class UpdateClassifyOutput : ClassifyDto, IOutputDto
    {
    }

	[AutoMapFrom(typeof(Classify))]
    public class GetClassifyOutput : ClassifyDto, IOutputDto
    {
    }

    [AutoMapFrom(typeof(Classify))]
    public class GetClassifyListOutput : ClassifyDto, IOutputDto
    {
    }
}


