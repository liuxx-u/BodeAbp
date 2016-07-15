using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace BodeAbp.Zero.Configuration
{
    public class BodeAbpZeroSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
                   {
                       new SettingDefinition(
                           BodeAbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                           "false",
                           new FixedLocalizableString("Is email confirmation required for login."),
                           scopes: SettingScopes.Application,
                           isVisibleToClients: true
                           ),
                       new SettingDefinition(
                           BodeAbpZeroSettingNames.OrganizationUnits.MaxUserMembershipCount,
                           int.MaxValue.ToString(),
                           new FixedLocalizableString("Maximum allowed organization unit membership count for a user."),
                           scopes: SettingScopes.Application,
                           isVisibleToClients: true
                           )
                   };
        }
    }
}
