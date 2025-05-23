using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Models
{
    public class ShopDetailViewModel
    {
        public Shop Shop { get; set; }
        public List<Shop> NearbyShops { get; set; }
        
        public ShopDetailViewModel()
        {
            NearbyShops = new List<Shop>();
        }
    }
}