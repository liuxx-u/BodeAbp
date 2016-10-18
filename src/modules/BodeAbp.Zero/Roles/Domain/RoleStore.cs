using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using BodeAbp.Zero.Permissions.Domain;
using Microsoft.AspNet.Identity;

namespace BodeAbp.Zero.Roles.Domain
{
    public class RoleStore :IQueryableRoleStore<Role, int>,IRolePermissionStore,ITransientDependency
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<RolePermission, long> _rolePermissionRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RoleStore(
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<RolePermission, long> rolePermissionRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public virtual IQueryable<Role> Roles
        {
            get { return _roleRepository.GetAll(); }
        }

        public virtual async Task CreateAsync(Role role)
        {
            await _roleRepository.InsertAsync(role);
        }

        public virtual async Task UpdateAsync(Role role)
        {
            await _roleRepository.UpdateAsync(role);
        }

        public virtual async Task DeleteAsync(Role role)
        {
            await _userRoleRepository.DeleteAsync(ur => ur.RoleId == role.Id);
            await _roleRepository.DeleteAsync(role);
        }

        public virtual async Task<Role> FindByIdAsync(int roleId)
        {
            return await _roleRepository.FirstOrDefaultAsync(roleId);
        }

        public virtual async Task<Role> FindByNameAsync(string roleName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.Name == roleName
                );
        }

        public virtual async Task<Role> FindByDisplayNameAsync(string displayName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.DisplayName == displayName
                );
        }

        /// <inheritdoc/>
        public virtual async Task AddPermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(role.Id, permissionGrant))
            {
                return;
            }

            await _rolePermissionRepository.InsertAsync(
                new RolePermission
                {
                    RoleId = role.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        /// <inheritdoc/>
        public virtual async Task RemovePermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            await _rolePermissionRepository.DeleteAsync(
                permissionSetting => permissionSetting.RoleId == role.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }

        /// <inheritdoc/>
        public virtual Task<IList<PermissionGrantInfo>> GetPermissionsAsync(Role role)
        {
            return GetPermissionsAsync(role.Id);
        }

        public async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(int roleId)
        {
            return (await _rolePermissionRepository.GetAllListAsync(p => p.RoleId == roleId))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        /// <inheritdoc/>
        public virtual async Task<bool> HasPermissionAsync(int roleId, PermissionGrantInfo permissionGrant)
        {
            return await _rolePermissionRepository.FirstOrDefaultAsync(
                p => p.RoleId == roleId &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        /// <inheritdoc/>
        public virtual async Task RemoveAllPermissionsAsync(Role role)
        {
            await _rolePermissionRepository.DeleteAsync(s => s.RoleId == role.Id);
        }

        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
