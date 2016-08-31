using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.BackgroundJobs;
using Abp.Extensions;
using BodeAbp.Zero.Navigations.Domain;

namespace WebDemo.Web.Areas.Admin.Controllers
{
    public class ZeroController : Controller
    {
        [Description("用户列表")]
        public ActionResult UserList()
        {
            return View();
        }

        [Description("角色列表")]
        public ActionResult RoleList()
        {
            return View();
        }

        [Description("组织机构列表")]
        public ActionResult OrganzationList()
        {
            return View();
        }

        [Description("系统设置")]
        public ActionResult SettingList()
        {
            return View();
        }

        [Description("审计日志")]
        public ActionResult AuditLogs()
        {
            return View();
        }

        [Description("后台任务列表")]
        public ActionResult BackgroundJobList()
        {
            ViewBag.Prioritys = typeof(BackgroundJobPriority).ToDictionary().Select(p => new
            {
                value = p.Key,
                text = p.Value
            }).ToArray();
            return View();
        }

        [Description("菜单列表")]
        public ActionResult NavigationList()
        {
            ViewBag.NavigationTypes = typeof(NavigationType).ToDictionary().Select(p => new ComboboxItemDto
            {
                Value = p.Key + "",
                DisplayText = p.Value
            }).ToArray();
            return View();
        }
    }
}