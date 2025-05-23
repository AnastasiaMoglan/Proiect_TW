using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Data.Helpers;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Initializer
{
    public class DbInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            // Seed initial admin user
            if (!context.Users.Any(u => u.Email == "admin@admin"))
            {
                context.Users.Add(new User
                {
                    Email = "admin@admin",
                    Username = "Administrator",
                    PasswordHash = PasswordHasher.HashPassword("admin1"),
                    Role = "Admin",
                    CreatedAt = DateTime.Now
                });
                context.SaveChanges();
            }

            // Seed accessories if none exist
            if (!context.Accessories.Any())
            {
                var accessories = new List<Accessory>
                {
                    // Cases
                    new Accessory { Name = "UrbanShield Hard Case", Category = "case", Price = 24.99m, ImagePath = "/Content/Accessories/case1.jpg", Description = "Durable hard case for maximum protection", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "VelvetTouch Soft Pouch", Category = "case", Price = 19.99m, ImagePath = "/Content/Accessories/case2.jpg", Description = "Soft microfiber pouch with plush lining", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "Compact Zip Case", Category = "case", Price = 22.99m, ImagePath = "/Content/Accessories/case3.jpg", Description = "Compact zipper case for easy storage", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "TravelPro Protective Box", Category = "case", Price = 29.99m, ImagePath = "/Content/Accessories/case4.jpg", Description = "Rigid protective box for travel", IsActive = true, CreatedAt = DateTime.Now },
                    
                    // Sprays
                    new Accessory { Name = "CrystalClear Cleaning Spray", Category = "spray", Price = 12.99m, ImagePath = "/Content/Accessories/spray1.jpg", Description = "Anti-streak cleaning solution", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "AntiFog Pro Solution", Category = "spray", Price = 14.99m, ImagePath = "/Content/Accessories/spray2.jpg", Description = "Long-lasting anti-fog formula", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "VisionFresh Mist", Category = "spray", Price = 11.99m, ImagePath = "/Content/Accessories/spray3.jpg", Description = "Refreshing lens cleaning mist", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "LensGuard Cleaner", Category = "spray", Price = 13.99m, ImagePath = "/Content/Accessories/spray4.jpg", Description = "Protective lens cleaner with UV guard", IsActive = true, CreatedAt = DateTime.Now },
                    
                    // Cloths
                    new Accessory { Name = "MicroSilk Lens Cloth", Category = "cloth", Price = 8.99m, ImagePath = "/Content/Accessories/cloth.jpg", Description = "Ultra-soft microfiber cleaning cloth", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "OptiWipe Premium", Category = "cloth", Price = 9.99m, ImagePath = "/Content/Accessories/cloth2.jpg", Description = "Premium lint-free cleaning cloth", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "GentleTouch Cleaner", Category = "cloth", Price = 7.99m, ImagePath = "/Content/Accessories/cloth3.jpg", Description = "Extra gentle cleaning cloth for sensitive lenses", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "ClearView Microfiber", Category = "cloth", Price = 8.99m, ImagePath = "/Content/Accessories/cloth4.jpg", Description = "High-absorption microfiber cloth", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "SmartShine Cloth", Category = "cloth", Price = 6.99m, ImagePath = "/Content/Accessories/cloth5.jpg", Description = "Basic cleaning cloth for daily use", IsActive = true, CreatedAt = DateTime.Now },
                    
                    // Cords & Chains
                    new Accessory { Name = "ClassicLoop Cord", Category = "cord", Price = 15.99m, ImagePath = "/Content/Accessories/cord1.jpg", Description = "Classic adjustable eyewear cord", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "GoldenLink Chain", Category = "cord", Price = 19.99m, ImagePath = "/Content/Accessories/cord2.jpg", Description = "Elegant gold-tone eyewear chain", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "SportFlex Strap", Category = "cord", Price = 16.99m, ImagePath = "/Content/Accessories/cord3.jpg", Description = "Flexible sports strap with secure grip", IsActive = true, CreatedAt = DateTime.Now },
                    new Accessory { Name = "PearlGrace Eyewear Chain", Category = "cord", Price = 24.99m, ImagePath = "/Content/Accessories/cord3.jpg", Description = "Decorative pearl-style eyewear chain", IsActive = true, CreatedAt = DateTime.Now }
                };

                context.Accessories.AddRange(accessories);
                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}