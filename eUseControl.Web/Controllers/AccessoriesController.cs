using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eUseControl.Data.Services;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly IAccessoryService _accessoryService;

        public AccessoriesController()
        {
            _accessoryService = new AccessoryService();
            
            // Ensure accessories are seeded in the database
            EnsureAccessoriesExist();
        }
        
        private void EnsureAccessoriesExist()
        {
            try
            {
                // Seed accessories if not already present
                _accessoryService.SeedAccessories();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error seeding accessories: " + ex.ToString());
                // Don't throw the exception - we'll let the database initializer handle this
                // This is just a fallback mechanism in case the initializer didn't run
            }
        }

        // GET: Accessories
        public ActionResult Index()
        {
            try
            {
                var viewModel = new AccessoriesViewModel
                {
                    AccessoriesByCategory = _accessoryService.GetAccessoriesByCategories()
                };
                
                // Get a few featured accessories (random selection for this example)
                var allAccessories = _accessoryService.GetAllAccessories();
                if (allAccessories.Count > 0)
                {
                    Random random = new Random();
                    viewModel.FeaturedAccessories = allAccessories
                        .OrderBy(x => random.Next())
                        .Take(Math.Min(4, allAccessories.Count))
                        .ToList();
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading accessories.";
                ViewBag.DetailedError = ex.Message;
                return View(new AccessoriesViewModel());
            }
        }
        
        // GET: Accessories/Category/case
        public ActionResult Category(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var accessories = _accessoryService.GetAccessoriesByCategory(id);
                ViewBag.CategoryName = GetCategoryDisplayName(id);
                return View(accessories);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading accessories for this category.";
                ViewBag.DetailedError = ex.Message;
                return View(new List<Accessory>());
            }
        }
        
        // GET: Accessories/Detail/5
        public ActionResult Detail(int id)
        {
            try
            {
                var accessory = _accessoryService.GetAccessoryById(id);
                
                if (accessory == null)
                {
                    return RedirectToAction("Index");
                }

                var viewModel = new AccessoryDetailViewModel
                {
                    Accessory = accessory,
                    RelatedAccessories = _accessoryService.GetAccessoriesByCategory(accessory.Category)
                        .Where(a => a.Id != accessory.Id)
                        .Take(4)
                        .ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the accessory details.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        private string GetCategoryDisplayName(string category)
        {
            switch (category.ToLower())
            {
                case "case": return "Eyewear Cases";
                case "spray": return "Cleaning Sprays";
                case "cloth": return "Cleaning Cloths";
                case "cord": return "Cords & Chains";
                default: return "Accessories";
            }
        }
    }
}