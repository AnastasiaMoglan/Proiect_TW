using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;

        public FavoritesController(IFavoriteService favoriteService, IProductService productService)
        {
            _favoriteService = favoriteService;
            _productService = productService;
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
                _favoriteService.RemoveFromFavorites(userId, productId);
                return Json(new { success = true });
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
                bool isNowFavorite = _favoriteService.IsProductFavorited(userId, productId);
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
                bool isFavorite = _favoriteService.IsProductFavorited(userId, productId);
                return Json(new { isFavorite }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private int GetCurrentUserId()
        {
            return Convert.ToInt32(Session["UserId"]);
        }
    }
}
