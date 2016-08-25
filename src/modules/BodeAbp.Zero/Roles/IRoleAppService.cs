using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.Roles.Dtos;

namespace BodeAbp.Zero.Roles
{
    /// <summary>
    /// 角色服务
    /// </summary>
    [Description("角色接口")]
    public interface IRoleAppService : IApplicationService
    {
        /// <summary>
        /// 获取 角色分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetRoleListOutput>> GetRolePagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetAllRoleNames();

        /// <summary>
        /// 获取 角色详情
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        Task<GetRoleOutput> GetRole(int id);

        /// <summary>
        /// 添加 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateRole(CreateRoleInput input);

        /// <summary>
        /// 更新 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateRole(UpdateRoleInput input);


        /// <summary>
        /// 删除 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteRole(List<IdInput> input);

        /// <summary>
        /// 获取已授权的权限名称
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>权限名称集合</returns>
        Task<IEnumerable<string>> GetRolePermissionNames(int roleId);

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="input">角色权限input</param>
        /// <returns>业务操作结果</returns>
        Task GrantPermissions(GrantRolePermissionInput input);

    }
}
