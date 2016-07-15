using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using BodeAbp.Zero.Permissions.Dtos;

namespace BodeAbp.Zero.Permissions
{
    /// <summary>
    /// 权限服务
    /// </summary>
    [Description("权限接口")]
    public interface IPermissionAppService : IApplicationService
    {
        /// <summary>
        /// 获取 所有权限
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<PermissionBriefOutPut> GetAllPermission();
    }
}
