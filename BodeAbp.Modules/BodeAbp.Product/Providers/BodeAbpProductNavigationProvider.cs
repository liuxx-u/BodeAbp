using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Localization;

namespace BodeAbp.Product.Providers
{
    public class BodeAbpProductNavigationProvider : NavigationProvider
    {
        /// <summary>
        /// 设置商品模块导航菜单
        /// </summary>
        /// <param name="context"></param>
        public override void SetNavigation(INavigationProviderContext context)
        {
            var sysMenu = new MenuItemDefinition("商品中心", L("商品中心"), "fa-asterisk");

            var classifyMenu = new MenuItemDefinition("分类管理", L("分类管理"), "fa-bookmark")
                .AddItem("分类列表", L("分类管理"), "fa-user", "/admin/attribute/classifyList");

            var attributeMenu = new MenuItemDefinition("属性管理", L("属性管理"), "fa-bookmark")
                    .AddItem("属性模板", L("属性模板"), "fa-user", "/admin/identity/userList")
                    .AddItem("属性选项", L("属性选项"), "plus", "/admin/identity/roleList")
                    .AddItem("SKU属性模板", L("SKU属性模板"), "plus", "/admin/identity/organzationList")
                    .AddItem("SKU属性选项", L("SKU属性选项"), "plus", "/admin/identity/organzationList");



            var productMenu = new MenuItemDefinition("商品管理", L("商品管理"), "fa-credit-card")
                .AddItem("新增商品", L("新增商品"), "plus", "/admin/setting/settingList")
                .AddItem("商品列表", L("商品列表"), "plus", "/admin/setting/auditLogs");


            var adminMenu = context.Manager.GetNavigationFromMenus("admin");
            adminMenu.AddItem(sysMenu
                .AddItem(classifyMenu)
                .AddItem(attributeMenu)
                .AddItem(productMenu));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpProductConsts.LocalizationSourceName);
        }

    }
}
