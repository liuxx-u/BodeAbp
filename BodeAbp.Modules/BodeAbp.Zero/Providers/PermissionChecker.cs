using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users.Domain;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace BodeAbp.Zero.Providers
{
    /// <summary>
    /// Application should inherit this class to implement <see cref="IPermissionChecker"/>.
    /// </summary>
    public class PermissionChecker : IPermissionChecker, ITransientDependency
    {
        private readonly AbpUserManager _userManager;

        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PermissionChecker(AbpUserManager userManager)
        {
            _userManager = userManager;
            AbpSession = NullAbpSession.Instance;
        }

        public async Task<bool> IsGrantedAsync(string permissionName)
        {
            return AbpSession.UserId.HasValue && await _userManager.IsGrantedAsync(AbpSession.UserId.Value, permissionName);
        }

        public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await _userManager.IsGrantedAsync(userId, permissionName);
        }
        
        public async Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
        {
            return await _userManager.IsGrantedAsync(user.UserId, permissionName);
        }
    }
}