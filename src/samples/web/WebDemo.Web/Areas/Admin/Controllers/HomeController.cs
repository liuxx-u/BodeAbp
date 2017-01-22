using System.Linq;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Extensions;
using BodeAbp.Product.Products.Domain;
using Abp.Web.Mvc.Authorization;
using BodeAbp.Product.Attributes.Domain;
using Abp.Domain.Repositories;
using BodeAbp.Activity.Activities.Domain;

namespace WebDemo.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<ProductClassify> _pclassifyRepository;
        private readonly IRepository<Classify> _classifyRepository;

        public HomeController(IRepository<ProductClassify> pclassifyRepository, IRepository<Classify> classifyRepository)
        {
            _pclassifyRepository = pclassifyRepository;
            _classifyRepository = classifyRepository;
        }

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
            //_pclassifyRepository.Insert(new ProductClassify() {Name="test" });
            //_classifyRepository.Insert(new Classify() { Name = "test" });
            return View();
        }

        #endregion
    }
}