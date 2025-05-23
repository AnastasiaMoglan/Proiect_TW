using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eUseControl.Data.Services;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ShopsController : Controller
    {
        private readonly IShopService _shopService;

        public ShopsController()
        {
            _shopService = new ShopService();
            
            EnsureShopsExist();
        }
        
        private void EnsureShopsExist()
        {
            try
            {
                _shopService.SeedShops();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error seeding shops: " + ex.ToString());
            }
        }
        
        public ActionResult Index()
        {
            try
            {
                var allShops = _shopService.GetAllShops();
                
                var cities = allShops.Select(s => s.City).Distinct().ToList();
                
                var featuredShops = allShops
                    .Where(s => s.IsServiceCenter || s.HasOptician)
                    .Take(3)
                    .ToList();
                
                var viewModel = new ShopsViewModel
                {
                    AllShops = allShops,
                    FeaturedShops = featuredShops,
                    Cities = cities
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading shops.";
                ViewBag.DetailedError = ex.Message;
                return View(new ShopsViewModel());
            }
        }
        
        public ActionResult Details(int id)
        {
            try
            {
                var shop = _shopService.GetShopById(id);
                
                if (shop == null)
                {
                    return RedirectToAction("Index");
                }
                
                var nearbyShops = _shopService.GetShopsByCity(shop.City)
                    .Where(s => s.Id != shop.Id)
                    .Take(3)
                    .ToList();
                
                var viewModel = new ShopDetailViewModel
                {
                    Shop = shop,
                    NearbyShops = nearbyShops
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading shop details.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        public ActionResult FilterByCity(string city)
        {
            try
            {
                var shops = string.IsNullOrEmpty(city) 
                    ? _shopService.GetAllShops() 
                    : _shopService.GetShopsByCity(city);
                
                var cities = _shopService.GetAllShops()
                    .Select(s => s.City)
                    .Distinct()
                    .ToList();
                
                var viewModel = new ShopsViewModel
                {
                    AllShops = shops,
                    Cities = cities
                };
                
                ViewBag.SelectedCity = city;
                
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while filtering shops.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        public ActionResult ServiceCenters()
        {
            try
            {
                var serviceCenters = _shopService.GetServiceCenters();
                
                var cities = _shopService.GetAllShops()
                    .Select(s => s.City)
                    .Distinct()
                    .ToList();
                
                var viewModel = new ShopsViewModel
                {
                    AllShops = serviceCenters,
                    Cities = cities
                };
                
                ViewBag.FilterType = "Service Centers";
                
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading service centers.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        public ActionResult WithOptician()
        {
            try
            {
                var shopsWithOptician = _shopService.GetShopsWithOptician();
                
                var cities = _shopService.GetAllShops()
                    .Select(s => s.City)
                    .Distinct()
                    .ToList();
                
                var viewModel = new ShopsViewModel
                {
                    AllShops = shopsWithOptician,
                    Cities = cities
                };
                
                ViewBag.FilterType = "Shops with Optician";
                
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading shops with opticians.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}