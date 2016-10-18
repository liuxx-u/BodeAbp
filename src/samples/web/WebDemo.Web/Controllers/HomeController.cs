﻿using System.Web.Mvc;
using Abp.Domain.Entities;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Mvc.Controllers;

namespace WebDemo.Web.Controllers
{
    //[AbpMvcAuthorize]
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }
	}
}