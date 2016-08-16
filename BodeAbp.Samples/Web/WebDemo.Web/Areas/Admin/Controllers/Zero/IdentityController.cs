using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDemo.Web.Areas.Admin.Controllers.Zero
{
    public class IdentityController : Controller
    {
        // GET: Admin/Identity
        public ActionResult OrganzationList()
        {
            return View();
        }
    }
}