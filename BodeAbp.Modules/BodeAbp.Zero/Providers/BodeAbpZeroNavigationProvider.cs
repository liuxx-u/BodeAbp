using System;
using Abp.Application.Navigation;
using Abp.Localization;

namespace BodeAbp.Zero.Providers
{
    public class BodeAbpZeroNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var adminMenu = context.Manager.GetNavigationFromMenus("admin");
            adminMenu.AddItem(new MenuItemDefinition("权限管理", L("权限管理"), "plus")
                    .AddItem(new MenuItemDefinition("用户列表", L("用户列表"), "plus", "/admin/identity/users"))
                    .AddItem(new MenuItemDefinition("角色列表", L("角色列表"), "plus", "/admin/identity/roles"))
                    .AddItem(new MenuItemDefinition("组织机构列表", L("组织机构列表"), "plus", "/admin/identity/organizationUnits"))
                    );

            adminMenu.AddItem(new MenuItemDefinition("日志管理", L("日志管理"), "plus")
                    .AddItem(new MenuItemDefinition("审计日志", L("审计日志"), "plus", "/admin/log/auditLogs")));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpZeroConsts.LocalizationSourceName);
        }
    }
}
