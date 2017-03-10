using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Localization;

namespace BodeAbp.Zero.Providers
{
    public class BodeAbpZeroSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var fileSettingGroup = new SettingDefinitionGroup(SettingNames.FileSettingGroupName, L("文件上传设置"));
            var basicSettingGroup= new SettingDefinitionGroup(SettingNames.BasicSettingGroupName, L("基础信息设置"));

            return new[]
            {
                new SettingDefinition(SettingNames.ImageSizeLimit, null, L("文件大小限制"),group:fileSettingGroup, scopes: SettingScopes.Application),
                new SettingDefinition(SettingNames.FileSuffixsLimit, null, L("文件类型限制"),group:fileSettingGroup, scopes: SettingScopes.Application),


                new SettingDefinition(SettingNames.IsEmailConfirmationRequiredForLogin,"false", L("登录是否开启邮箱验证"),group:basicSettingGroup,scopes: SettingScopes.Application),
                new SettingDefinition(SettingNames.MaxUserMembershipCount,int.MaxValue.ToString(), L("组织机构最大用户数"),group:basicSettingGroup,scopes: SettingScopes.Application)
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpZeroConsts.LocalizationSourceName);
        }
    }
}
