using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly Dictionary<string, decimal> productPrices = new Dictionary<string, decimal>
        {
            // Cases
            {"UrbanShield Hard Case", 24.99m},
            {"VelvetTouch Soft Pouch", 19.99m},
            {"Compact Zip Case", 22.99m},
            {"TravelPro Protective Box", 29.99m},
            // Sprays
            {"CrystalClear Cleaning Spray", 12.99m},
            {"AntiFog Pro Solution", 14.99m},
            {"VisionFresh Mist", 11.99m},
            {"LensGuard Cleaner", 13.99m},
            // Cloths
            {"MicroSilk Lens Cloth", 8.99m},
            {"OptiWipe Premium", 9.99m},
            {"GentleTouch Cleaner", 7.99m},
            {"ClearView Microfiber", 8.99m},
            {"SmartShine Cloth", 6.99m},
            // Cords & Chains
            {"ClassicLoop Cord", 15.99m},
            {"GoldenLink Chain", 19.99m},
            {"SportFlex Strap", 16.99m},
            {"PearlGrace Eyewear Chain", 24.99m}
        };

        // GET: Accessories
        public ActionResult Index()
        {
            // Get all image files from Content/Accessories
            var accessoriesDir = Server.MapPath("~/Content/Accessories");
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var files = Directory.GetFiles(accessoriesDir)
                .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLower()))
                .Select(f => Path.GetFileName(f))
                .ToList();

            var products = files.Select(f =>
            {
                var name = GetProductName(f);
                return new AccessoriesProduct
                {
                    Image = "/Content/Accessories/" + f,
                    Category = InferCategory(f),
                    Name = name,
                    Price = productPrices.ContainsKey(name) ? productPrices[name] : 0m
                };
            }).ToList();

            return View(products);
        }

        private string InferCategory(string filename)
        {
            var lower = filename.ToLower();
            if (lower.StartsWith("case")) return "case";
            if (lower.StartsWith("spray")) return "spray";
            if (lower.StartsWith("cloth")) return "cloth";
            if (lower.StartsWith("cord")) return "cord";
            return "other";
        }

        private string GetProductName(string filename)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(filename).ToLower();
            switch (name)
            {
                case "case1": return "UrbanShield Hard Case";
                case "case2": return "VelvetTouch Soft Pouch";
                case "case3": return "Compact Zip Case";
                case "case4": return "TravelPro Protective Box";
                case "spray1": return "CrystalClear Cleaning Spray";
                case "spray2": return "AntiFog Pro Solution";
                case "spray3": return "VisionFresh Mist";
                case "spray4": return "LensGuard Cleaner";
                case "cloth": return "MicroSilk Lens Cloth";
                case "cloth2": return "OptiWipe Premium";
                case "cloth3": return "GentleTouch Cleaner";
                case "cloth4": return "ClearView Microfiber";
                case "cloth5": return "SmartShine Cloth";
                case "cord1": return "ClassicLoop Cord";
                case "cord2": return "GoldenLink Chain";
                case "cord3": return "SportFlex Strap";
                default: return name;
            }
        }
    }
} 