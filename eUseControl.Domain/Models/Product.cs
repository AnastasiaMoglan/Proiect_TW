using System;

namespace eUseControl.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Money Price { get; set; }
        public Money DiscountedPrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public ProductType Type { get; set; }
        public string Gender { get; set; }
        public string VisionType { get; set; }
    }
} 