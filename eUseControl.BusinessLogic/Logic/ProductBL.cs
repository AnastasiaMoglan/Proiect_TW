using System.Collections.Generic;
using System.Linq;
using eUseControl.Domain.Models;

namespace eUseControl.BusinessLogic.Logic
{
    public class ProductBL
    {
        // Simulated product store (in a real app, this would come from a database)
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Classic Aviator", Description = "Timeless aviator style with UV protection.", Price = new Money(100, "$"), ImageUrl = "/Content/Sunglasses/soare1.jpg", Type = ProductType.Sunglasses, Gender = "Men" },
            new Product { Id = 2, Name = "Retro Round", Description = "Vintage round frames for a classic look.", Price = new Money(120, "$"), ImageUrl = "/Content/Sunglasses/soare2.jpg", Type = ProductType.Sunglasses, Gender = "Women" },
            new Product { Id = 3, Name = "Modern Square", Description = "Bold square design, perfect for any outfit.", Price = new Money(124, "$"), ImageUrl = "/Content/Sunglasses/soare3.jpg", Type = ProductType.Sunglasses, Gender = "Men" },
            new Product { Id = 4, Name = "Sporty Wrap", Description = "Sporty wrap-around for active lifestyles.", Price = new Money(156, "$"), ImageUrl = "/Content/Sunglasses/soare4.jpg", Type = ProductType.Sunglasses, Gender = "Men" },
            new Product { Id = 5, Name = "Vintage Cat Eye", Description = "Chic cat eye frames for a retro vibe.", Price = new Money(434, "$"), ImageUrl = "/Content/Sunglasses/soare5.jpg", Type = ProductType.Sunglasses, Gender = "Women" },
            new Product { Id = 6, Name = "Trendy Oversized", Description = "Oversized lenses for maximum coverage.", Price = new Money(123, "$"), ImageUrl = "/Content/Sunglasses/soare6.jpg", Type = ProductType.Sunglasses, Gender = "Women" },
            new Product { Id = 7, Name = "Urban Classic", Description = "Sleek and timeless for everyday wear.", Price = new Money(140, "$"), ImageUrl = "/Content/Frames/rame1.jpg", Type = ProductType.OpticalFrames, VisionType = "Near" },
            new Product { Id = 8, Name = "Elegant Oval", Description = "Elegant oval frames for a refined look.", Price = new Money(178, "$"), ImageUrl = "/Content/Frames/rame2.jpg", Type = ProductType.OpticalFrames, VisionType = "Distance" },
            new Product { Id = 9, Name = "Bold Rectangle", Description = "Bold rectangular design for a modern style.", Price = new Money(167, "$"), ImageUrl = "/Content/Frames/rame3.jpg", Type = ProductType.OpticalFrames, VisionType = "Near" },
            new Product { Id = 10, Name = "Chic Cat Eye", Description = "Chic cat eye frames for a fashionable statement.", Price = new Money(189, "$"), ImageUrl = "/Content/Frames/rame4.jpg", Type = ProductType.OpticalFrames, VisionType = "Distance" },
            new Product { Id = 11, Name = "Minimalist Round", Description = "Minimalist round frames for a subtle touch.", Price = new Money(177, "$"), ImageUrl = "/Content/Frames/rame5.jpg", Type = ProductType.OpticalFrames, VisionType = "Near" },
            new Product { Id = 12, Name = "Trendy Square", Description = "Trendy square frames for a bold impression.", Price = new Money(134, "$"), ImageUrl = "/Content/Frames/rame6.jpg", Type = ProductType.OpticalFrames, VisionType = "Distance" },
            // Accessories examples
            new Product { Id = 13, Name = "Cleaning Kit", Description = "Complete cleaning kit for your glasses.", Price = new Money(25, "$"), ImageUrl = "/Content/Accessories/cleaningkit.jpg", Type = ProductType.Accessories },
            new Product { Id = 14, Name = "Hard Case", Description = "Durable hard case for glasses protection.", Price = new Money(40, "$"), ImageUrl = "/Content/Accessories/hardcase.jpg", Type = ProductType.Accessories },
            // Lenses examples
            new Product { Id = 15, Name = "Blue Light Lenses", Description = "Protect your eyes from blue light.", Price = new Money(80, "$"), ImageUrl = "/Content/Lenses/Lentila1.png", Type = ProductType.Lenses },
            new Product { Id = 16, Name = "Photochromic Lenses", Description = "Lenses that darken in sunlight.", Price = new Money(120, "$"), ImageUrl = "/Content/Lenses/Lentila2.png", Type = ProductType.Lenses }
        };

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public List<Product> GetProductsByType(ProductType type)
        {
            return _products.Where(p => p.Type == type).ToList();
        }

        public Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }
} 