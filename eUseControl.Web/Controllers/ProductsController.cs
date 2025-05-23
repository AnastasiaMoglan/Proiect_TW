using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eUseControl.Data.Services;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController()
        {
            _productService = new ProductService();
            
            EnsureProductsExist();
        }
        
        private void EnsureProductsExist()
        {
            try
            {
                _productService.SeedProducts();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error seeding products: " + ex.ToString());
            }
        }
        
        public ActionResult Index()
        {
            try
            {
                var viewModel = new ProductsViewModel
                {
                    FeaturedSunglasses = _productService.GetSunglasses().Take(3).ToList(),
                    FeaturedOpticalFrames = _productService.GetOpticalFrames().Take(3).ToList(),
                    FeaturedLenses = _productService.GetLenses().Take(3).ToList()
                };

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading products.";
                ViewBag.DetailedError = ex.Message;
                return View("Index", new ProductsViewModel());
            }
        }

        public ActionResult Sunglasses()
        {
            try
            {
                var sunglasses = _productService.GetSunglasses();
                return View(sunglasses);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading sunglasses.";
                ViewBag.DetailedError = ex.Message;
                return View(new List<Product>());
            }
        }

        public ActionResult OpticalFrames()
        {
            try
            {
                var frames = _productService.GetOpticalFrames();
                return View(frames);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading optical frames.";
                ViewBag.DetailedError = ex.Message;
                return View(new List<Product>());
            }
        }

        public ActionResult Lenses()
        {
            try
            {
                var lenses = _productService.GetLenses();
                return View(lenses);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading lenses.";
                ViewBag.DetailedError = ex.Message;
                return View(new List<Product>());
            }
        }
        
        public ActionResult Detail(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                
                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                var relatedProducts = new List<Product>();
                switch (product.Type)
                {
                    case "Sunglasses":
                        relatedProducts = _productService.GetSunglasses();
                        break;
                    case "OpticalFrames":
                        relatedProducts = _productService.GetOpticalFrames();
                        break;
                    case "Lenses":
                        relatedProducts = _productService.GetLenses();
                        break;
                }

                var viewModel = new ProductDetailViewModel
                {
                    Product = product,
                    RelatedProducts = relatedProducts
                        .Where(p => p.Id != product.Id)
                        .Take(4)
                        .ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the product details.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}