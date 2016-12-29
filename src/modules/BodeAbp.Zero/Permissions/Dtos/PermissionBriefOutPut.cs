using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Localization;

namespace BodeAbp.Zero.Permissions.Dtos
{
    /// <summary>
    /// 权限基本信息
    /// </summary>
    [AutoMapFrom(typeof(Permission))]
    public class PermissionBriefOutPut
    {
        /// <summary>
        /// 权限名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public ILocalizableString DisplayName { get; set; }

        /// <summary>
        /// 子权限集合
        /// </summary>
        public IReadOnlyList<PermissionBriefOutPut> Children { get; set; }
    }
}
