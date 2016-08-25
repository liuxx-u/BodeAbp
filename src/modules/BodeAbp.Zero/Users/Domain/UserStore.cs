using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BodeAbp.Zero.Permissions.Domain;
using BodeAbp.Zero.Roles.Domain;
using Microsoft.AspNet.Identity;

namespace BodeAbp.Zero.Users.Domain
{
    public class UserStore:
        IUserPasswordStore<User, long>,
        IUserEmailStore<User, long>,
        IUserLoginStore<User, long>,
        IUserRoleStore<User, long>,
        IQueryableUserStore<User, long>,
        IUserPermissionStore,
        
        ITransientDependency
    {

        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserExternalLogin, long> _userExternalLoginRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPermission, long> _userPermissionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public UserStore(
            IRepository<User, long> userRepository,
            IRepository<UserExternalLogin, long> userExternalLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermission, long> userPermissionRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userRepository = userRepository;
            _userExternalLoginRepository = userExternalLoginRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task CreateAsync(User user)
        {
            await _userRepository.InsertAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(User user)
        {
            await _userRepository.DeleteAsync(user.Id);
        }

        public async Task<User> FindByIdAsync(long userId)
        {
            return await _userRepository.FirstOrDefaultAsync(userId);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.UserName == userName
                );
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.EmailAddress == email
                );
        }

        /// <summary>
        /// Tries to find a user with user name or email address in current tenant.
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        public async Task<User> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => (user.UserName == userNameOrEmailAddress || user.EmailAddress == userNameOrEmailAddress)
                );
        }

        /// <summary>
        /// Tries to find a user with user name or email address in given tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        [UnitOfWork]
        public async Task<User> FindByNameOrEmailAsync(int? tenantId, string userNameOrEmailAddress)
        {
            return await FindByNameOrEmailAsync(userNameOrEmailAddress);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        public Task SetEmailAsync(User user, string email)
        {
            user.EmailAddress = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(user.EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(user.IsEmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            user.IsEmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task AddLoginAsync(User user, UserLoginInfo login)
        {
            await _userExternalLoginRepository.InsertAsync(
                new UserExternalLogin
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });
        }

        public async Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            await _userExternalLoginRepository.DeleteAsync(
                ul => ul.UserId == user.Id &&
                      ul.LoginProvider == login.LoginProvider &&
                      ul.ProviderKey == login.ProviderKey
                );
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            return (await _userExternalLoginRepository.GetAllListAsync(ul => ul.UserId == user.Id))
                .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                .ToList();
        }

        public async Task<User> FindAsync(UserLoginInfo login)
        {
            var userLogin = await _userExternalLoginRepository.FirstOrDefaultAsync(
                ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey
                );

            if (userLogin == null)
            {
                return null;
            }

            return await _userRepository.FirstOrDefaultAsync(u => u.Id == userLogin.UserId);
        }

        [UnitOfWork]
        public Task<List<User>> FindAllAsync(UserLoginInfo login)
        {
            var query = from userLogin in _userExternalLoginRepository.GetAll()
                        join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                        where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                        select user;

            return Task.FromResult(query.ToList());
        }

        public Task<User> FindAsync(int? tenantId, UserLoginInfo login)
        {
            var query = from userLogin in _userExternalLoginRepository.GetAll()
                        join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                        where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                        select user;

            return Task.FromResult(query.FirstOrDefault());
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            await _userRoleRepository.InsertAsync(
                new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            if (userRole == null)
            {
                return;
            }

            await _userRoleRepository.DeleteAsync(userRole);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            //TODO: This is not implemented as async.
            var roleNames = _userRoleRepository.Query(userRoles => (from userRole in userRoles
                                                                    join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                                                                    where userRole.UserId == user.Id
                                                                    select role.Name).ToList());

            return Task.FromResult<IList<string>>(roleNames);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            return await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id) != null;
        }

        public IQueryable<User> Users
        {
            get { return _userRepository.GetAll(); }
        }

        public async Task AddPermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(user.Id, permissionGrant))
            {
                return;
            }

            await _userPermissionRepository.InsertAsync(
                new UserPermission
                {
                    UserId = user.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        public async Task RemovePermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            await _userPermissionRepository.DeleteAsync(
                permissionSetting => permissionSetting.UserId == user.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }

        public async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(long userId)
        {
            return (await _userPermissionRepository.GetAllListAsync(p => p.UserId == userId))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        public async Task<bool> HasPermissionAsync(long userId, PermissionGrantInfo permissionGrant)
        {
            return await _userPermissionRepository.FirstOrDefaultAsync(
                p => p.UserId == userId &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        public async Task RemoveAllPermissionSettingsAsync(User user)
        {
            await _userPermissionRepository.DeleteAsync(s => s.UserId == user.Id);
        }

        public void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
