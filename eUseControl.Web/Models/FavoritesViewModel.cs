using System;
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Models
{
    public class FavoritesViewModel
    {
        public List<FavoriteItem> FavoriteItems { get; set; }
        public List<Favorite> Favorites { get; set; }

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