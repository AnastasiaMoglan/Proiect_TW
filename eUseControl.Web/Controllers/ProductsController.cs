using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class ProductsController : Controller
    {
            // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sunglasses()
        {
            return View();
        }

        public ActionResult OpticalFrames()
        {
            return View();
        }
    }
}