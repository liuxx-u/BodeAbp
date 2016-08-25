using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using BodeAbp.Zero.Configuration;
using BodeAbp.Zero.Providers;

namespace BodeAbp.Zero.Organizations.Domain
{
    /// <summary>
    /// Implements <see cref="IOrganizationUnitSettings"/> to get settings from <see cref="ISettingManager"/>.
    /// </summary>
    public class OrganizationUnitSettings : IOrganizationUnitSettings, ITransientDependency
    {
        /// <summary>
        /// Maximum allowed organization unit membership count for a user.
        /// Returns value for current tenant.
        /// </summary>
        public int MaxUserMembershipCount
        {
            get
            {
                return _settingManager.GetSettingValue<int>(SettingNames.MaxUserMembershipCount);
            }
        }

        private readonly ISettingManager _settingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUnitSettings"/> class.
        /// </summary>
        public OrganizationUnitSettings(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        /// <summary>
        /// Maximum allowed organization unit membership count for a user.
        /// Returns value for given tenant.
        /// </summary>
        public async Task<int> GetMaxUserMembershipCountAsync()
        {
            return await _settingManager.GetSettingValueForApplicationAsync<int>(SettingNames.MaxUserMembershipCount);
        }

        public async Task SetMaxUserMembershipCountAsync(int value)
        {
            await _settingManager.ChangeSettingForApplicationAsync(SettingNames.MaxUserMembershipCount, value.ToString());
        }
    }
}