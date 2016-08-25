using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace BodeAbp.Zero.Users.Dtos
{
    /// <summary>
    /// 设置用户角色Input
    /// </summary>
    public class SetUserRoleInput: IInputDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色名称集合
        /// </summary>
        public string[] RoleNames { get; set; }
    }
}
