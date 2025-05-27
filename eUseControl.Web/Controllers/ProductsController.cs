using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        // Dependency Injection via constructor
        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
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
                var sunglasses = (minPrice == null && maxPrice == null)
                    ? _productService.GetSunglasses()
                    : _productService.FilterSunglasses(minPrice, maxPrice);

                return View("Sunglasses", sunglasses.ToList());
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
                var opticalFrames = (minPrice == null && maxPrice == null)
                    ? _productService.GetOpticalFrames()
                    : _productService.FilterOpticalFrames(minPrice, maxPrice);

                return View("OpticalFrames", opticalFrames.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while filtering Optical Frames.";
                ViewBag.DetailedError = ex.Message;
                return View("OpticalFrames", new List<Product>());
            }
        }

        public ActionResult Lenses(decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                var lenses = (minPrice == null && maxPrice == null)
                    ? _productService.GetLenses()
                    : _productService.FilterLenses(minPrice, maxPrice);

                return View("Lenses", lenses.ToList());
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
                    return RedirectToAction("Index");

                var relatedProducts = _productService.GetProductsByType(product.Type)
                    .Where(p => p.Id != product.Id)
                    .Take(4)
                    .ToList();

                var viewModel = new ProductDetailViewModel
                {
                    Product = product,
                    RelatedProducts = relatedProducts
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
