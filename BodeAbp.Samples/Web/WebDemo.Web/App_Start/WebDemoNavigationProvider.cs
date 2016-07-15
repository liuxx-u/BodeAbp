using Abp.Application.Navigation;
using Abp.Localization;

namespace WebDemo.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class WebDemoNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var adminMenu = context.Manager.GetNavigationFromMenus("admin");
            adminMenu.AddItem(new MenuItemDefinition("Dashboard",L("HomePage"),url: "/admin",icon: "fa fa-home"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, WebDemoConsts.LocalizationSourceName);
        }
    }
}
