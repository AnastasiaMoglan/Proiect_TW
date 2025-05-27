using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class AccessoryService : IAccessoryService
    {
        private readonly IAccessoryRepository _accessoryRepository;

        public AccessoryService(IAccessoryRepository accessoryRepository)
        {
            _accessoryRepository = accessoryRepository ?? throw new ArgumentNullException(nameof(accessoryRepository));
        }

        public List<Accessory> GetAllAccessories()
        {
            return _accessoryRepository.GetAllAccessories();
        }

        public List<Accessory> GetAccessoriesByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category must be provided.", nameof(category));

            return _accessoryRepository.GetAccessoriesByCategory(category);
        }

        public Accessory GetAccessoryById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid accessory ID.", nameof(id));

            return _accessoryRepository.GetAccessoryById(id);
        }

        public Accessory GetAccessoryByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be provided.", nameof(name));

            return _accessoryRepository.GetAccessoryByName(name);
        }

        public void AddAccessory(Accessory accessory)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccessory(Accessory accessory)
        {
            throw new NotImplementedException();
        }

        public void AddAccessory(string name, string category, decimal price, string imagePath, string description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be empty", nameof(category));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero", nameof(price));

            if (string.IsNullOrWhiteSpace(imagePath))
                throw new ArgumentException("Image path cannot be empty", nameof(imagePath));

            if (_accessoryRepository.AccessoryExists(name))
                throw new InvalidOperationException($"Accessory '{name}' already exists");

            var accessory = new Accessory
            {
                Name = name,
                Category = category,
                Price = price,
                ImagePath = imagePath,
                Description = description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _accessoryRepository.AddAccessory(accessory);
            _accessoryRepository.SaveChanges();
        }

        public void UpdateAccessory(int id, string name, string category, decimal price, string imagePath, string description = null)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid accessory ID.", nameof(id));

            var accessory = _accessoryRepository.GetAccessoryById(id)
                ?? throw new InvalidOperationException($"Accessory with ID {id} not found");

            if (!string.IsNullOrWhiteSpace(name)) accessory.Name = name;
            if (!string.IsNullOrWhiteSpace(category)) accessory.Category = category;
            if (price > 0) accessory.Price = price;
            if (!string.IsNullOrWhiteSpace(imagePath)) accessory.ImagePath = imagePath;

            accessory.Description = description;
            accessory.UpdatedAt = DateTime.UtcNow;

            _accessoryRepository.UpdateAccessory(accessory);
            _accessoryRepository.SaveChanges();
        }

        public void DeleteAccessory(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid accessory ID.", nameof(id));

            _accessoryRepository.DeleteAccessory(id);
            _accessoryRepository.SaveChanges();
        }

        public Dictionary<string, List<Accessory>> GetAccessoriesByCategories()
        {
            return GetAllAccessories()
                .GroupBy(a => a.Category?.ToLower() ?? string.Empty)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public void SeedAccessories()
        {
            if (_accessoryRepository.GetAllAccessories().Any()) return;

            var accessories = new List<Accessory>
            {
                new Accessory { Name = "UrbanShield Hard Case", Category = "case", Price = 24.99m, ImagePath = "/Content/Accessories/case1.jpg", Description = "Durable hard case", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Accessory { Name = "VelvetTouch Soft Pouch", Category = "case", Price = 19.99m, ImagePath = "/Content/Accessories/case2.jpg", Description = "Soft microfiber pouch", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Accessory { Name = "CrystalClear Cleaning Spray", Category = "spray", Price = 12.99m, ImagePath = "/Content/Accessories/spray1.jpg", Description = "Anti-streak spray", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Accessory { Name = "MicroSilk Lens Cloth", Category = "cloth", Price = 8.99m, ImagePath = "/Content/Accessories/cloth.jpg", Description = "Ultra-soft cloth", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Accessory { Name = "ClassicLoop Cord", Category = "cord", Price = 15.99m, ImagePath = "/Content/Accessories/cord1.jpg", Description = "Adjustable eyewear cord", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            try
            {
                foreach (var accessory in accessories)
                {
                    if (!_accessoryRepository.AccessoryExists(accessory.Name))
                        _accessoryRepository.AddAccessory(accessory);
                }

                _accessoryRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error seeding accessories: " + ex);
                throw;
            }
        }
    }
}
