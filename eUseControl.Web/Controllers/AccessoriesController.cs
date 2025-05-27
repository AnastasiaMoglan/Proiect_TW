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
    public class AccessoriesController : Controller
    {
        private readonly IAccessoryService _accessoryService;

        public AccessoriesController(IAccessoryService accessoryService)
        {
            _accessoryService = accessoryService ?? throw new ArgumentNullException(nameof(accessoryService));
        }
        public ActionResult Index()
        {
            try
            {
                var allAccessories = _accessoryService.GetAllAccessories();
                var featured = allAccessories
                    .OrderBy(x => Guid.NewGuid())
                    .Take(Math.Min(4, allAccessories.Count))
                    .ToList();

                var viewModel = new AccessoriesViewModel
                {
                    AccessoriesByCategory = _accessoryService.GetAccessoriesByCategories(),
                    FeaturedAccessories = featured
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading accessories.";
                ViewBag.DetailedError = ex.Message;
                return View(new AccessoriesViewModel());
            }
        }

        public ActionResult Category(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index");

            try
            {
                var accessories = _accessoryService.GetAccessoriesByCategory(id);
                ViewBag.CategoryName = GetCategoryDisplayName(id);
                return View(accessories);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading category.";
                ViewBag.DetailedError = ex.Message;
                return View(new List<Accessory>());
            }
        }

        public ActionResult Detail(int id)
        {
            try
            {
                var accessory = _accessoryService.GetAccessoryById(id);
                if (accessory == null)
                    return RedirectToAction("Index");

                var related = _accessoryService.GetAccessoriesByCategory(accessory.Category)
                    .Where(a => a.Id != accessory.Id)
                    .Take(4)
                    .ToList();

                var viewModel = new AccessoryDetailViewModel
                {
                    Accessory = accessory,
                    RelatedAccessories = related
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading accessory details.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }

        private string GetCategoryDisplayName(string category)
        {
            return category?.ToLower() switch
            {
                "case" => "Eyewear Cases",
                "spray" => "Cleaning Sprays",
                "cloth" => "Cleaning Cloths",
                "cord" => "Cords & Chains",
                _ => "Accessories"
            };
        }
    }
}
