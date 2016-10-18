using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Extensions;
using BodeAbp.Product.Attributes.Domain;
using System.Threading.Tasks;
using Abp.Dependency;
using BodeAbp.Product.Attributes;

namespace WebDemo.Web.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult AttributeList()
        {
            ViewBag.AttributeTypes = typeof(ProductAttributeType).ToDictionary().Select(p => new
            {
                value = p.Key,
                text = p.Value
            }).ToList();
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

        public async Task<ActionResult> AddProduct(int classifyId)
        {
            ViewBag.ClassifyId = classifyId;
            var attributesService = IocManager.Instance.Resolve<IAttributesAppService>();
            ViewBag.Classifies = await attributesService.GetClassifySelectedOptions();
            return View();
        }

        public ActionResult ProductList()
        {
            return View();
        }


    }
}