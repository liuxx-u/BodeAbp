using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace BodeAbp.Product.Providers
{
    public class BodeAbpProductSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(SettingNames.SettingName, null, L("SettingName"), scopes: SettingScopes.Application, isVisibleToClients: true)
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpProductConsts.LocalizationSourceName);
        }
    }
}
