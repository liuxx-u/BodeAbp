using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Rpc.Coordinate.Zookeeper.Setting
{
    internal class ZoomKeeperSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var settingGroup = new SettingDefinitionGroup(ZoomKeeperSettingNames.GroupName, L("ZoomKeeper设置"));
            return new[]
                   {
                       new SettingDefinition(ZoomKeeperSettingNames.ConnectionString, "", L("连接字符串"),group:settingGroup, scopes: SettingScopes.Application),
                       new SettingDefinition(ZoomKeeperSettingNames.RoutePath, "/dotnet/serviceRoutes", L("路由配置路径"), group:settingGroup,scopes: SettingScopes.Application),
                       new SettingDefinition(ZoomKeeperSettingNames.SessionTimeout, "20", L("会话超时时间"), group:settingGroup,description:L("（单位：秒）"),scopes: SettingScopes.Application),
                       new SettingDefinition(ZoomKeeperSettingNames.ChRoot, null, L("根节点"), group:settingGroup,scopes: SettingScopes.Application)
                   };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}
