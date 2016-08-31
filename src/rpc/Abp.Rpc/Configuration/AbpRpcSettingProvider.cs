using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Rpc.Configuration
{
    public class AbpRpcSettingProvider: SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var rpcSettingGroup = new SettingDefinitionGroup(RpcSettingNames.GroupName, L("RPC设置"));

            return new[]
            {
                new SettingDefinition(RpcSettingNames.RpcFilePath, @"d:\routes.txt", L("RpcFilePath"),rpcSettingGroup, scopes: SettingScopes.Application)
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpRpcConsts.LocalizationSourceName);
        }
    }
}
