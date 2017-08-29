using System;
using System.Text;
using System.Web.Mvc;
using Abp.Domain.Entities;
using Abp.Helper;
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

        public ActionResult DesTest()
        {
            var str= DesHelper.Encrypt("http://user.cczcrv.com/", "12345678");
            return Content(str);
        }

        public ActionResult Base64Test()
        {
            var result= Convert.ToBase64String(Encoding.UTF8.GetBytes("liuxx001")).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return Content(result);
        }
    }
}