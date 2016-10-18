using Abp.AutoMapper;
using BodeAbp.Zero.Navigations.Domain;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BodeAbp.Zero.Navigations.Dtos
{
    public abstract class NavigationDto : EntityDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 父级Id，0表示一级分类
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        public NavigationType NavigationType { get; set; }
    }

	[AutoMapTo(typeof(Navigation))]
    public class NavigationInput : NavigationDto
    {
    }

    [AutoMapFrom(typeof(Navigation))]
    public class NavigationListOutput : NavigationDto
    {
        public NavigationListOutput()
        {
            Children = new List<NavigationListOutput>();
        }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ICollection<NavigationListOutput> Children { get; set; }
    }
}


