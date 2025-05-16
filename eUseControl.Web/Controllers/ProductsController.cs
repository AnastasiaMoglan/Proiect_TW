using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eUseControl.Domain.Models;

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
            var sunglasses = new List<Product>
            {
                new Product { Id = 1, Name = "Classic Aviator", Description = "Timeless aviator style with UV protection.", Price = new Money(100, "Lei"), ImageUrl = Url.Content("~/Content/Sunglasses/soare1.jpg"), Type = ProductType.Sunglasses, Gender = "Men" },
                new Product { Id = 2, Name = "Retro Round", Description = "Vintage round frames for a classic look.", Price = new Money(120, "Lei"), ImageUrl = Url.Content("~/Content/Sunglasses/soare2.jpg"), Type = ProductType.Sunglasses, Gender = "Women" },
                new Product { Id = 3, Name = "Modern Square", Description = "Bold square design, perfect for any outfit.", Price = new Money(124, "Lei"), ImageUrl = Url.Content("~/Content/Sunglasses/soare3.jpg"), Type = ProductType.Sunglasses, Gender = "Men" },
                new Product { Id = 4, Name = "Sporty Wrap", Description = "Sporty wrap-around for active lifestyles.", Price = new Money(156, "Lei"), ImageUrl = Url.Content("~/Content/Sunglasses/soare4.jpg"), Type = ProductType.Sunglasses, Gender = "Men" },
                new Product { Id = 5, Name = "Vintage Cat Eye", Description = "Chic cat eye frames for a retro vibe.", Price = new Money(434, "Lei"), ImageUrl = Url.Content("~/Content/Sunglasses/soare5.jpg"), Type = ProductType.Sunglasses, Gender = "Women" },
                new Product { Id = 6, Name = "Trendy Oversized", Description = "Oversized lenses for maximum coverage.", Price = new Money(123, "Lei"), ImageUrl = Url.Content("~/Content/Sunglasses/soare6.jpg"), Type = ProductType.Sunglasses, Gender = "Women" }
            };
            return View(sunglasses);
        }

        public ActionResult OpticalFrames()
        {
            var frames = new List<Product>
            {
                new Product { Id = 1, Name = "Urban Classic", Description = "Sleek and timeless for everyday wear.", Price = new Money(140, "USD"), ImageUrl = Url.Content("~/Content/Frames/rame1.jpg"), Type = ProductType.OpticalFrames, VisionType = "Near" },
                new Product { Id = 2, Name = "Elegant Oval", Description = "Elegant oval frames for a refined look.", Price = new Money(178, "USD"), ImageUrl = Url.Content("~/Content/Frames/rame2.jpg"), Type = ProductType.OpticalFrames, VisionType = "Distance" },
                new Product { Id = 3, Name = "Bold Rectangle", Description = "Bold rectangular design for a modern style.", Price = new Money(167, "USD"), ImageUrl = Url.Content("~/Content/Frames/rame3.jpg"), Type = ProductType.OpticalFrames, VisionType = "Near" },
                new Product { Id = 4, Name = "Chic Cat Eye", Description = "Chic cat eye frames for a fashionable statement.", Price = new Money(189, "USD"), ImageUrl = Url.Content("~/Content/Frames/rame4.jpg"), Type = ProductType.OpticalFrames, VisionType = "Distance" },
                new Product { Id = 5, Name = "Minimalist Round", Description = "Minimalist round frames for a subtle touch.", Price = new Money(177, "USD"), ImageUrl = Url.Content("~/Content/Frames/rame5.jpg"), Type = ProductType.OpticalFrames, VisionType = "Near" },
                new Product { Id = 6, Name = "Trendy Square", Description = "Trendy square frames for a bold impression.", Price = new Money(134, "USD"), ImageUrl = Url.Content("~/Content/Frames/rame6.jpg"), Type = ProductType.OpticalFrames, VisionType = "Distance" }
            };
            return View(frames);
        }

        public ActionResult Lenses()
        {
            return View();
        }
    }
}