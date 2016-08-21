using Abp.Application.Navigation;
using Abp.Localization;

namespace BodeAbp.Zero.Providers
{
    public class BodeAbpZeroNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var adminMenu = context.Manager.GetNavigationFromMenus("admin");

            var identityMenu = new MenuItemDefinition("权限管理", L("权限管理"), "fa-bookmark")
                    .AddItem("用户列表", L("用户列表"), "fa-user", "/admin/identity/userList")
                    .AddItem("角色列表", L("角色列表"), "plus", "/admin/identity/roleList")
                    .AddItem("组织机构列表", L("组织机构列表"), "plus", "/admin/identity/organzationList");

            var logMenu = new MenuItemDefinition("基础设置", L("基础设置"), "fa-credit-card")
                .AddItem("设置列表", L("设置列表"), "plus", "/admin/setting/settingList")
                .AddItem("审计日志", L("审计日志"), "plus", "/admin/setting/auditLogs");


            adminMenu.AddItem(new MenuItemDefinition("系统管理", L("系统管理"), "fa-asterisk")
                .AddItem(identityMenu)
                .AddItem(logMenu));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpZeroConsts.LocalizationSourceName);
        }
    }
}
