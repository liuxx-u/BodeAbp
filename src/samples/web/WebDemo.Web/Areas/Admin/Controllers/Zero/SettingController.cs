using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.BackgroundJobs;
using Abp.Extensions;

namespace WebDemo.Web.Areas.Admin.Controllers.Zero
{
    public class SettingController : Controller
    {
        // GET: Admin/Setting
        public ActionResult SettingList()
        {
            return View();
        }

        public ActionResult AuditLogs()
        {
            return View();
        }

        public ActionResult BackgroundJobList()
        {
            ViewBag.Prioritys = typeof(BackgroundJobPriority).ToDictionary().Select(p => new
            {
                value = p.Key,
                text = p.Value
            }).ToList();
            return View();
        }
    }
}