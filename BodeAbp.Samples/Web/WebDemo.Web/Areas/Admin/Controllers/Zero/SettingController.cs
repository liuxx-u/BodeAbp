using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}