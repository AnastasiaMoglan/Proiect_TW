using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Models
{
    public class ProductsViewModel
    {
        public List<Product> FeaturedSunglasses { get; set; }
        public List<Product> FeaturedOpticalFrames { get; set; }
        public List<Product> FeaturedLenses { get; set; }
        
        public ProductsViewModel()
        {
            FeaturedSunglasses = new List<Product>();
            FeaturedOpticalFrames = new List<Product>();
            FeaturedLenses = new List<Product>();
        }
    }
}