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

        public ActionResult Sunglasses(decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                List<Product> sunglasses;
                if (minPrice == null && maxPrice == null)
                {
                    sunglasses = _productService.GetSunglasses();
                }
                else
                {
                    sunglasses = _productService.FilterSunglasses(minPrice, maxPrice);
                }
                return View("Sunglasses", sunglasses);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while filtering Sunglasses.";
                ViewBag.DetailedError = ex.Message;
                return View("Sunglasses", new List<Product>());
            }
        }

        public ActionResult OpticalFrames(decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                List<Product> opticalFrames;;
                if (minPrice == null && maxPrice == null)
                {
                    opticalFrames = _productService.GetOpticalFrames();
                }
                else
                {
                    opticalFrames = _productService.FilterOpticalFrames(minPrice, maxPrice);
                }
                return View("OpticalFrames", opticalFrames);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while filtering OpticalFrames.";
                ViewBag.DetailedError = ex.Message;
                return View("OpticalFrames", new List<Product>());
            }
        }

        public ActionResult Lenses(decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                List<Product> lenses;;
                if (minPrice == null && maxPrice == null)
                {
                    lenses = _productService.GetLenses();
                }
                else
                {
                    lenses = _productService.FilterLenses(minPrice, maxPrice);
                }
                return View("Lenses", lenses);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while filtering Lenses.";
                ViewBag.DetailedError = ex.Message;
                return View("Lenses", new List<Product>());
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