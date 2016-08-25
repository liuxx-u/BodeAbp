using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using BodeAbp.Zero.Roles.Domain;

namespace BodeAbp.Zero.Roles.Dtos
{
    public abstract class RoleQueryDto : EntityDto
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        [MaxLength(Role.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 角色显示名.
        /// </summary>
        [Required]
        [StringLength(Role.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否静态角色
        /// 静态属性不能删除，不能修改
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 是否默认属性
        /// 创建用户时自动添加默认角色
        /// </summary>
        public bool IsDefault { get; set; }
    }
	
	[AutoMapFrom(typeof(Role))]
    public class GetRoleOutput : RoleQueryDto, IOutputDto
    {
    }

	[AutoMapFrom(typeof(Role))]
    public class GetRoleListOutput : RoleQueryDto, IOutputDto
    {
    }
}


