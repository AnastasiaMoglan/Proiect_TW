using System;
using System.Collections.Generic;

namespace eUseControl.Web.Models
{
    public class FavoritesViewModel
    {
        public List<FavoriteItem> FavoriteItems { get; set; }

        public FavoritesViewModel()
        {
            FavoriteItems = new List<FavoriteItem>();
        }
    }

    public class FavoriteItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public bool InStock { get; set; } = true;
    }
}