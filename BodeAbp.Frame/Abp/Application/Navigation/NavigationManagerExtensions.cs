using Abp.Localization;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// NavigationManager扩展类
    /// </summary>
    public static class NavigationManagerExtensions
    {
        /// <summary>
        /// 根据Key获取菜单，没有则创建并添加
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static MenuDefinition GetNavigationFromMenus(this INavigationManager manager,string key)
        {
            MenuDefinition menu;
            if (!manager.Menus.TryGetValue("admin", out menu))
            {
                menu = new MenuDefinition(key, L("key"));
                manager.Menus.Add("admin", menu);
            }
            return menu;
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}
