using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Controllers;

namespace WebDemo.Web.Areas.Admin.Controllers.Product
{
    public class AttributeController : AbpController
    {
        // GET: Admin/Attribute
        public ActionResult Index()
        {
            return View();
        }
    }
}