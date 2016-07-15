using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services;
using Abp.AutoMapper;
using BodeAbp.Zero.Permissions.Dtos;

namespace BodeAbp.Zero.Permissions
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        /// <summary>
        /// 获取 所有权限
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<PermissionBriefOutPut> GetAllPermission()
        {
            var permissions = PermissionManager.GetAllPermissions().Where(p => p.Parent == null).ToList();
            return permissions.MapTo<IReadOnlyList<PermissionBriefOutPut>>();
        }
    }
}
