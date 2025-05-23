using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using eUseControl.Data.Repository;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Services
{
    public class AccessoryService : IAccessoryService
    {
        private readonly IAccessoryRepository _accessoryRepository;

        public AccessoryService()
        {
            _accessoryRepository = new AccessoryRepository();
        }

        public AccessoryService(IAccessoryRepository accessoryRepository)
        {
            _accessoryRepository = accessoryRepository;
        }

        public List<Accessory> GetAllAccessories()
        {
            return _accessoryRepository.GetAllAccessories();
        }

        public List<Accessory> GetAccessoriesByCategory(string category)
        {
            return _accessoryRepository.GetAccessoriesByCategory(category);
        }

        public Accessory GetAccessoryById(int id)
        {
            return _accessoryRepository.GetAccessoryById(id);
        }

        public Accessory GetAccessoryByName(string name)
        {
            return _accessoryRepository.GetAccessoryByName(name);
        }

        public void AddAccessory(string name, string category, decimal price, string imagePath, string description = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrEmpty(category))
                throw new ArgumentException("Category cannot be empty", nameof(category));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero", nameof(price));

            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentException("Image path cannot be empty", nameof(imagePath));

            if (_accessoryRepository.AccessoryExists(name))
                throw new InvalidOperationException($"Accessory with name '{name}' already exists");

            var accessory = new Accessory
            {
                Name = name,
                Category = category,
                Price = price,
                ImagePath = imagePath,
                Description = description,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _accessoryRepository.AddAccessory(accessory);
            _accessoryRepository.SaveChanges();
        }

        public void UpdateAccessory(int id, string name, string category, decimal price, string imagePath, string description = null)
        {
            var accessory = _accessoryRepository.GetAccessoryById(id);
            if (accessory == null)
                throw new InvalidOperationException($"Accessory with ID {id} not found");

            if (!string.IsNullOrEmpty(name))
                accessory.Name = name;

            if (!string.IsNullOrEmpty(category))
                accessory.Category = category;

            if (price > 0)
                accessory.Price = price;

            if (!string.IsNullOrEmpty(imagePath))
                accessory.ImagePath = imagePath;

            accessory.Description = description;
            accessory.UpdatedAt = DateTime.Now;

            _accessoryRepository.UpdateAccessory(accessory);
            _accessoryRepository.SaveChanges();
        }

        public void DeleteAccessory(int id)
        {
            _accessoryRepository.DeleteAccessory(id);
            _accessoryRepository.SaveChanges();
        }

        public Dictionary<string, List<Accessory>> GetAccessoriesByCategories()
        {
            var accessories = GetAllAccessories();
            return accessories.GroupBy(a => a.Category.ToLower())
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public void SeedAccessories()
        {
            // Only seed if there are no accessories in the database
            if (_accessoryRepository.GetAllAccessories().Any())
                return;

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
        
            try
            {
                foreach (var accessory in accessories)
                {
                    if (!_accessoryRepository.AccessoryExists(accessory.Name))
                    {
                        _accessoryRepository.AddAccessory(accessory);
                    }
                }
        
                _accessoryRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error seeding accessories: " + ex.ToString());
                throw;
            }
        }
    }
}
