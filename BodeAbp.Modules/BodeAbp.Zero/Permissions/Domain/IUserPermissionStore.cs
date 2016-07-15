using System.Collections.Generic;
using System.Threading.Tasks;
using BodeAbp.Zero.Users.Domain;

namespace BodeAbp.Zero.Permissions.Domain
{
    public interface IUserPermissionStore
    {
        /// <summary>
        /// Adds a permission grant setting to a user.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        Task AddPermissionAsync(User user, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Removes a permission grant setting from a user.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        Task RemovePermissionAsync(User user, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Gets permission grant setting informations for a user.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of permission setting informations</returns>
        Task<IList<PermissionGrantInfo>> GetPermissionsAsync(long userId);

        /// <summary>
        /// Checks whether a role has a permission grant setting info.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(long userId, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Deleted all permission settings for a role.
        /// </summary>
        /// <param name="user">User</param>
        Task RemoveAllPermissionSettingsAsync(User user);
    }
}
