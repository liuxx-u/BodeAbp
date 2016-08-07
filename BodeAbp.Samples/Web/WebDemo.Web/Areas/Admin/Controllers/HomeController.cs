using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Extensions;
using BodeAbp.Product.Products.Domain;
using WebDemo.Web.Areas.Admin.ViewModels;

namespace WebDemo.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        #region 操作方法

        public ActionResult GetMenus()
        {
            var menus = new []
            {
                new { MenuName="管理信息",ClassName="Myicon001",Childrens=""},
                new { MenuName="其他设置",ClassName="Myicon002",Childrens=""},
                 new { MenuName="编辑器",ClassName="Myicon003",Childrens=""},
            };
            return Json(menus, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 视图页
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Table()
        {
            ViewBag.Enums = typeof(EnumTest).ToDictionary().Select(p => new
            {
                value = p.Key,
                text = p.Value
            }).ToList();
            return View();
        }

        public ActionResult _LeftMenu()
        {
            NavigationViewModel navigation = new NavigationViewModel
            {
                Name="1",
                DisplayName="工作台",
                ClassName= "Myicon001",
                Children=new List<NavigationViewModel>()
                {
                    new NavigationViewModel { Name="1-1",DisplayName="管理信息",ClassName="",Children=new List<NavigationViewModel>()
                    {
                        new NavigationViewModel { Name="1-1-1",DisplayName="首页模板",ClassName=""},
                        new NavigationViewModel { Name="1-1-2",DisplayName="其他设置",ClassName=""},
                        new NavigationViewModel { Name="1-1-3",DisplayName="编辑器",ClassName=""},
                        new NavigationViewModel { Name="1-1-4",DisplayName="日期管理",ClassName=""}
                    } },
                    new NavigationViewModel { Name="1-2",DisplayName="其他设置",ClassName=""},
                    new NavigationViewModel { Name="1-3",DisplayName="编辑器",ClassName=""},
                    new NavigationViewModel { Name="1-4",DisplayName="日期管理",ClassName=""},
                }
            };
            return View(navigation);
        }

        public ActionResult Default()
        {
            return View();
        }

        #endregion
    }
}