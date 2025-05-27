using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;

        public FavoritesController(IFavoriteService favoriteService, IProductService productService)
        {
            _favoriteService = favoriteService ?? throw new ArgumentNullException(nameof(favoriteService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }
        public ActionResult Index()
        {
            try
            {
                int userId = GetCurrentUserId();
                var dtoList = _favoriteService.GetUserFavorites(userId);

                var viewModel = new FavoritesViewModel
                {
                    FavoriteItems = dtoList.Select(dto => new FavoriteItem
                    {
                        ProductId = dto.ProductId,
                        ProductName = dto.Product?.Name ?? "Unknown",
                        Price = dto.Product?.Price ?? 0,
                        ImageUrl = dto.Product?.ImageUrl,
                        InStock = true,
                        DateAdded = dto.DateAdded
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading favorites.";
                ViewBag.DetailedError = ex.Message;
                return View(new FavoritesViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToFavorites(int productId)
        {
            try
            {
                int userId = GetCurrentUserId();
                _favoriteService.AddToFavorites(userId, productId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromFavorites(int productId)
        {
            try
            {
                int userId = GetCurrentUserId();

                // Corectează să folosești produsul pentru a afla favoriteId, dacă e nevoie
                var favorite = _favoriteService.GetUserFavorites(userId)
                    .FirstOrDefault(f => f.ProductId == productId);

                if (favorite != null)
                {
                    _favoriteService.RemoveFromFavorites(userId, favorite.Id);
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Favorite not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleFavorite(int productId)
        {
            try
            {
                int userId = GetCurrentUserId();
                _favoriteService.ToggleFavorite(userId, productId);

                bool isNowFavorite = _favoriteService.IsFavorite(userId, productId);
                return Json(new { success = true, isFavorite = isNowFavorite });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult IsFavorite(int productId)
        {
            try
            {
                int userId = GetCurrentUserId();
                bool isFavorite = _favoriteService.IsFavorite(userId, productId);
                return Json(new { isFavorite }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private int GetCurrentUserId()
        {
            if (Session["UserId"] == null)
                throw new Exception("User not logged in or session expired.");
            return Convert.ToInt32(Session["UserId"]);
        }
    }
}
