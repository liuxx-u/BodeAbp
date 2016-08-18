using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDemo.Web.Areas.Admin.Controllers.Zero
{
    public class IdentityController : Controller
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
    }
}