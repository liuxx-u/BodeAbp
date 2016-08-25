using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Helper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using BodeAbp.Zero.Roles.Domain;
using BodeAbp.Zero.Roles.Dtos;

namespace BodeAbp.Zero.Roles
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleRepository"></param>
        /// <param name="roleManager"></param>
        public RoleAppService(IRepository<Role> roleRepository,RoleManager roleManager)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
        }


        /// <summary>
        /// 获取 角色分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultOutput<GetRoleListOutput>> GetRolePagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _roleRepository.QueryWithNoTracking().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetRoleListOutput>(total, list.MapTo<List<GetRoleListOutput>>());
        }

        /// <summary>
        /// 获取 角色详情
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        public async Task<GetRoleOutput> GetRole(int id)
        {
            var role = await _roleManager.GetRoleByIdAsync(id);
            return role.MapTo<GetRoleOutput>();
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetAllRoleNames()
        {
            var roles = await  _roleRepository.GetAll().
                Select(p => new { RoleName = p.Name, DisplayName = p.DisplayName }).ToListAsync();
            return roles;
        }

        /// <summary>
        /// 添加 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateRole(CreateRoleInput input)
        {
            var role = input.MapTo<Role>();
            await _roleManager.CreateAsync(role);
        }

        /// <summary>
        /// 更新 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateRole(UpdateRoleInput input)
        {
            var role = await _roleRepository.GetAsync(input.Id);
            input.MapTo(role);
            await _roleManager.UpdateAsync(role);
        }

        /// <summary>
        /// 删除 角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteRole(List<IdInput> input)
        {
            var ids = input.Select(p => p.Id);
            await _roleRepository.DeleteAsync(p => ids.Contains(p.Id));
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="input">角色权限input</param>
        /// <returns>业务操作结果</returns>
        public async Task GrantPermissions(GrantRolePermissionInput input)
        {
            var permissions= PermissionManager.GetAllPermissions().Where(p => input.PermissionNames.Contains(p.Name));

            //自动添加父级权限
            var fullPermissions = new List<Permission>(permissions);
            foreach (var permission in permissions)
            {
                var parent = permission.Parent;
                while (parent != null)
                {
                    if (!fullPermissions.Contains(parent, EqualityHelper<Permission>.CreateComparer(p=>p.Name)))
                    {
                        fullPermissions.Add(parent);
                    }
                    parent = parent.Parent;
                }
            }

            await _roleManager.SetGrantedPermissionsAsync(input.RoleId, fullPermissions);
        }

        /// <summary>
        /// 获取已授权的权限名称
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>权限名称集合</returns>
        public async Task<IEnumerable<string>> GetRolePermissionNames(int roleId)
        {
            //只返回leaf权限
            return (await _roleManager.GetGrantedPermissionsAsync(roleId)).Where(p=>!p.Children.Any()).Select(p => p.Name);
        }
    }
}
