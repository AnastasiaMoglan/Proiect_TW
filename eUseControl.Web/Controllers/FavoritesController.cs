using System.Collections.Generic;
using System.Web.Mvc;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class FavoritesController : Controller
    {
        // GET: Favorites
        public ActionResult Index()
        {
            // In a real application, you would retrieve the user's favorites from a database
            var model = new FavoritesViewModel
            {
                FavoriteItems = GetSampleFavorites()
            };
            
            return View(model);
        }
        
        // POST: Favorites/Remove/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int id)
        {
            // In a real application, you would remove the item from the user's favorites in the database
            
            TempData["SuccessMessage"] = "Item removed from favorites.";
            return RedirectToAction("Index");
        }
        
        // POST: Favorites/AddToCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int id)
        {
            // In a real application, you would add the item to the user's cart in the database
            
            TempData["SuccessMessage"] = "Item added to cart.";
            return RedirectToAction("Index");
        }
        
        // Helper method to generate sample data
        private List<FavoriteItem> GetSampleFavorites()
        {
            return new List<FavoriteItem>
            {
                new FavoriteItem
                {
                    ProductId = 101,
                    ProductName = "Premium Contact Lenses",
                    Price = 49.99m,
                    ImageUrl = "/Content/Lenses/Lentila1.png",
                    DateAdded = System.DateTime.Now.AddDays(-10)
                },
                new FavoriteItem
                {
                    ProductId = 305,
                    ProductName = "Designer Frames - Ray-Ban",
                    Price = 180.50m,
                    ImageUrl = "/Content/Frames/rame1.jpg",
                    DateAdded = System.DateTime.Now.AddDays(-5)
                },
                new FavoriteItem
                {
                    ProductId = 402,
                    ProductName = "Deluxe Glasses Case",
                    Price = 30.00m,
                    ImageUrl = "/Content/Accessories/Case1.png",
                    DateAdded = System.DateTime.Now.AddDays(-2)
                }
            };
        }
    }
}