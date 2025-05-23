using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository()
        {
            _db = new AppDbContext();
        }

        public List<Product> GetAllProducts()
        {
            return _db.Products.Where(p => p.IsActive).ToList();
        }

        public List<Product> GetProductsByType(string type)
        {
            return _db.Products.Where(p => p.Type == type && p.IsActive).ToList();
        }

        public Product GetProductById(int id)
        {
            return _db.Products.FirstOrDefault(p => p.Id == id && p.IsActive);
        }

        public void AddProduct(Product product)
        {
            product.CreatedAt = DateTime.Now;
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _db.Products.Find(product.Id);
            
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Type = product.Type;
                existingProduct.Price = product.Price;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.Description = product.Description;
                existingProduct.Gender = product.Gender;
                existingProduct.VisionType = product.VisionType;
                existingProduct.IsActive = product.IsActive;
                existingProduct.UpdatedAt = DateTime.Now;
                
                _db.Entry(existingProduct).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            var product = _db.Products.Find(id);
            
            if (product != null)
            {
                product.IsActive = false;
                product.UpdatedAt = DateTime.Now;
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void SeedProducts()
        {
            if (!_db.Products.Any())
            {
                // Sunglasses
                var sunglasses = new List<Product>
                {
                    new Product { Name = "Classic Aviator", Type = "Sunglasses", Price = 100, ImageUrl = "~/Content/Sunglasses/soare1.jpg", Description = "Timeless aviator style with UV protection.", Gender = "Men" },
                    new Product { Name = "Retro Round", Type = "Sunglasses", Price = 120, ImageUrl = "~/Content/Sunglasses/soare2.jpg", Description = "Vintage round frames for a classic look.", Gender = "Women" },
                    new Product { Name = "Modern Square", Type = "Sunglasses", Price = 124, ImageUrl = "~/Content/Sunglasses/soare3.jpg", Description = "Bold square design, perfect for any outfit.", Gender = "Men" },
                    new Product { Name = "Sporty Wrap", Type = "Sunglasses", Price = 156, ImageUrl = "~/Content/Sunglasses/soare4.jpg", Description = "Sporty wrap-around for active lifestyles.", Gender = "Men" },
                    new Product { Name = "Vintage Cat Eye", Type = "Sunglasses", Price = 434, ImageUrl = "~/Content/Sunglasses/soare5.jpg", Description = "Chic cat eye frames for a retro vibe.", Gender = "Women" },
                    new Product { Name = "Trendy Oversized", Type = "Sunglasses", Price = 123, ImageUrl = "~/Content/Sunglasses/soare6.jpg", Description = "Oversized lenses for maximum coverage.", Gender = "Women" }
                };

                // Optical Frames
                var frames = new List<Product>
                {
                    new Product { Name = "Urban Classic", Type = "OpticalFrames", Price = 140, ImageUrl = "~/Content/Frames/rame1.jpg", Description = "Sleek and timeless for everyday wear.", VisionType = "Near" },
                    new Product { Name = "Elegant Oval", Type = "OpticalFrames", Price = 178, ImageUrl = "~/Content/Frames/rame2.jpg", Description = "Elegant oval frames for a refined look.", VisionType = "Distance" },
                    new Product { Name = "Bold Rectangle", Type = "OpticalFrames", Price = 167, ImageUrl = "~/Content/Frames/rame3.jpg", Description = "Bold rectangular design for a modern style.", VisionType = "Near" },
                    new Product { Name = "Chic Cat Eye", Type = "OpticalFrames", Price = 189, ImageUrl = "~/Content/Frames/rame4.jpg", Description = "Chic cat eye frames for a fashionable statement.", VisionType = "Distance" },
                    new Product { Name = "Minimalist Round", Type = "OpticalFrames", Price = 177, ImageUrl = "~/Content/Frames/rame5.jpg", Description = "Minimalist round frames for a subtle touch.", VisionType = "Near" },
                    new Product { Name = "Trendy Square", Type = "OpticalFrames", Price = 134, ImageUrl = "~/Content/Frames/rame6.jpg", Description = "Trendy square frames for a bold impression.", VisionType = "Distance" }
                };

                // Lenses
                var lenses = new List<Product>
                {
                    new Product { Name = "Essilor Single Vision", Type = "Lenses", Price = 120, ImageUrl = "~/Content/Lenses/Lentila1.png", Description = "Clear single vision lenses with UV protection" },
                    new Product { Name = "Zeiss Progressive", Type = "Lenses", Price = 280, ImageUrl = "~/Content/Lenses/Lentila2.png", Description = "Premium progressive lenses with digital design" },
                    new Product { Name = "Hoya Bifocal", Type = "Lenses", Price = 160, ImageUrl = "~/Content/Lenses/Lentila3.png", Description = "Traditional bifocal lenses with clear segment" },
                    new Product { Name = "Varilux Progressive", Type = "Lenses", Price = 320, ImageUrl = "~/Content/Lenses/Lentila4.png", Description = "Advanced progressive lenses with smooth transitions" },
                    new Product { Name = "Eyezen Single Vision", Type = "Lenses", Price = 140, ImageUrl = "~/Content/Lenses/Lentila6.png", Description = "Digital-friendly lenses for daily eye comfort" }
                };

                _db.Products.AddRange(sunglasses);
                _db.Products.AddRange(frames);
                _db.Products.AddRange(lenses);
                _db.SaveChanges();
            }
        }
    }
}