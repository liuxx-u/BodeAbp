using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Net.Mail
{
    /// <summary>
    /// Defines settings to send emails.
    /// <see cref="EmailSettingNames"/> for all available configurations.
    /// </summary>
    internal class EmailSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var emailSettingGroup = new SettingDefinitionGroup(EmailSettingNames.GroupName, L("MailSettingGroupName"));
            return new[]
                   {
                       new SettingDefinition(EmailSettingNames.Smtp.Host, "127.0.0.1", L("SmtpHost"),group:emailSettingGroup, scopes: SettingScopes.Application),
                       new SettingDefinition(EmailSettingNames.Smtp.Port, "25", L("SmtpPort"), group:emailSettingGroup,scopes: SettingScopes.Application),
                       new SettingDefinition(EmailSettingNames.Smtp.UserName, "", L("Username"), group:emailSettingGroup,scopes: SettingScopes.Application),
                       new SettingDefinition(EmailSettingNames.Smtp.Password, "", L("Password"), group:emailSettingGroup,scopes: SettingScopes.Application),
                       new SettingDefinition(EmailSettingNames.Smtp.Domain, "", L("DomainName"), group:emailSettingGroup,scopes: SettingScopes.Application),
                       new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, "false", L("UseSSL"),group:emailSettingGroup, scopes: SettingScopes.Application),
                       new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, "true", L("UseDefaultCredentials"),group:emailSettingGroup, scopes: SettingScopes.Application),
                       new SettingDefinition(EmailSettingNames.DefaultFromAddress, "", L("DefaultFromSenderEmailAddress"), group:emailSettingGroup,scopes: SettingScopes.Application),
                       new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, "", L("DefaultFromSenderDisplayName"), group:emailSettingGroup,scopes: SettingScopes.Application)
                   };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}