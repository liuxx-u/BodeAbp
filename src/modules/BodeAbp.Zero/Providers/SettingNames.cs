using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodeAbp.Zero.Providers
{
    internal class SettingNames
    {
        public const string FileSettingGroupName = "Zero.File.SettingGroupName";

        public const string FileSizeLimit = "Zero.File.SizeLimit";

        public const string FileSuffixsLimit = "Zero.File.SuffixsLimit";


        public const string BasicSettingGroupName = "Zero.Basic.SettingGroupName";
        /// <summary>
        /// "Abp.Zero.UserManagement.IsEmailConfirmationRequiredForLogin".
        /// </summary>
        public const string IsEmailConfirmationRequiredForLogin = "Zero.UserManagement.IsEmailConfirmationRequiredForLogin";

        /// <summary>
        /// "Abp.Zero.OrganizationUnits.MaxUserMembershipCount".
        /// </summary>
        public const string MaxUserMembershipCount = "Zero.OrganizationUnits.MaxUserMembershipCount";
    }
}
