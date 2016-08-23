using Abp.Application.Navigation;
using Abp.Localization;

namespace BodeAbp.Zero.Providers
{
    public class BodeAbpZeroNavigationProvider : NavigationProvider
    {
        /// <summary>
        /// 设置Zero模块导航菜单
        /// </summary>
        /// <param name="context"></param>
        public override void SetNavigation(INavigationProviderContext context)
        {
            var sysMenu = new MenuItemDefinition("系统设置", L("系统设置"), "fa-asterisk");

            var identityMenu = new MenuItemDefinition("权限管理", L("权限管理"), "fa-bookmark")
                    .AddItem("用户列表", L("用户列表"), "fa-user", "/admin/identity/userList")
                    .AddItem("角色列表", L("角色列表"), "plus", "/admin/identity/roleList")
                    .AddItem("组织机构列表", L("组织机构列表"), "plus", "/admin/identity/organzationList");

            var logMenu = new MenuItemDefinition("基础信息", L("基础信息"), "fa-credit-card")
                .AddItem("系统设置", L("系统设置"), "plus", "/admin/setting/settingList")
                .AddItem("审计日志", L("审计日志"), "plus", "/admin/setting/auditLogs")
                .AddItem("后台任务", L("后台任务"), "plus", "/admin/setting/backgroundJobList");


            var adminMenu = context.Manager.GetNavigationFromMenus("admin");
            adminMenu.AddItem(sysMenu.AddItem(identityMenu).AddItem(logMenu));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpZeroConsts.LocalizationSourceName);
        }
    }
}
