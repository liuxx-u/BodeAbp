using System.Linq;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Extensions;
using BodeAbp.Product.Products.Domain;
using Abp.Web.Mvc.Authorization;

namespace WebDemo.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        #region 视图页
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Table()
        {
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }

        #endregion
    }
}