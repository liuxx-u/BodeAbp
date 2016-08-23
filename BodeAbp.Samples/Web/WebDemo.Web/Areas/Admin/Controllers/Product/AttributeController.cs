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
        public ActionResult AttributeList()
        {
            return View();
        }

        public ActionResult AttributeOptionList()
        {
            return View();
        }

        public ActionResult SkuAttributeList()
        {
            return View();
        }

        public ActionResult SkuAttributeOptionList()
        {
            return View();
        }

        public ActionResult ClassifyList()
        {
            return View();
        }
    }
}