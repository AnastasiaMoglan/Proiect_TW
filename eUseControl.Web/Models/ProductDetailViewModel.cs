using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Models
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
        
        public ProductDetailViewModel()
        {
            RelatedProducts = new List<Product>();
        }
    }
}