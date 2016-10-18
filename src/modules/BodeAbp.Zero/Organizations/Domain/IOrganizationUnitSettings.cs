using System.Threading.Tasks;

namespace BodeAbp.Zero.Organizations.Domain
{
    /// <summary>
    /// Used to get settings related to OrganizationUnits.
    /// </summary>
    public interface IOrganizationUnitSettings
    {
        /// <summary>
        /// GetsMaximum allowed organization unit membership count for a user.
        /// Returns value for current tenant.
        /// </summary>
        int MaxUserMembershipCount { get; }

        /// <summary>
        /// Gets Maximum allowed organization unit membership count for a user.
        /// Returns value for given tenant.
        /// </summary>
        Task<int> GetMaxUserMembershipCountAsync();

        /// <summary>
        /// Sets Maximum allowed organization unit membership count for a user.
        /// </summary>
        /// <param name="value">Setting value.</param>
        /// <returns></returns>
        Task SetMaxUserMembershipCountAsync(int value);
    }
}