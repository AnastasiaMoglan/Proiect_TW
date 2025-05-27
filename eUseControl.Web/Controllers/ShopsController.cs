using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Interfaces;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ShopsController : Controller
    {
        private readonly IShopService _shopService;

        // Injecție de dependență prin constructor (corectă pentru testabilitate și IoC)
        public ShopsController(IShopService shopService)
        {
            _shopService = shopService ?? throw new ArgumentNullException(nameof(shopService));
        }

        // Pagina principală afișând toate magazinele, orașele distincte și magazinele recomandate
        public ActionResult Index()
        {
            try
            {
                var allShops = _shopService.GetAllShops();

                var cities = allShops
                    .Select(s => s.City)
                    .Distinct()
                    .ToList();

                var featuredShops = allShops
                    .Where(s => s.IsServiceCenter || s.HasOptician)
                    .Take(3)
                    .ToList();

                var viewModel = new ShopsViewModel
                {
                    AllShops = allShops,
                    Cities = cities,
                    FeaturedShops = featuredShops
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "A apărut o eroare la încărcarea magazinelor.";
                ViewBag.DetailedError = ex.Message;
                return View(new ShopsViewModel());
            }
        }

        // Detaliile unui magazin după ID
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
                ViewBag.ErrorMessage = "A apărut o eroare la încărcarea detaliilor magazinului.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Filtrare magazine după oraș
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
                    Cities = cities,
                    SelectedCity = city
                };

                ViewBag.SelectedCity = city;

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "A apărut o eroare la filtrarea magazinelor.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Filtrare magazine care sunt centre de service
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
                    Cities = cities,
                    FilterType = "Service Centers"
                };

                ViewBag.FilterType = "Service Centers";

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "A apărut o eroare la încărcarea centrelor de service.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Filtrare magazine care au optician
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
                    Cities = cities,
                    FilterType = "Shops with Optician"
                };

                ViewBag.FilterType = "Shops with Optician";

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "A apărut o eroare la încărcarea magazinelor cu optician.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
