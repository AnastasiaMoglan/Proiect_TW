using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Models
{
    public class ShopsViewModel
    {
        public List<Shop> AllShops { get; set; }
        public List<Shop> FeaturedShops { get; set; }
        public List<string> Cities { get; set; }
        public string SelectedCity { get; set; }
        public string FilterType { get; set; }

        public ShopsViewModel()
        {
            AllShops = new List<Shop>();
            FeaturedShops = new List<Shop>();
            Cities = new List<string>();
        }
    }
}