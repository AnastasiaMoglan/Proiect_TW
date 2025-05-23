using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Repository
{
    public class AccessoryRepository : IAccessoryRepository
    {
        private readonly AppDbContext _context;

        public AccessoryRepository()
        {
            _context = new AppDbContext();
        }

        public List<Accessory> GetAllAccessories()
        {
            return _context.Accessories.ToList();
        }

        public List<Accessory> GetAccessoriesByCategory(string category)
        {
            return _context.Accessories.Where(a => a.Category.ToLower() == category.ToLower()).ToList();
        }

        public Accessory GetAccessoryById(int id)
        {
            return _context.Accessories.Find(id);
        }

        public Accessory GetAccessoryByName(string name)
        {
            return _context.Accessories.FirstOrDefault(a => a.Name.ToLower() == name.ToLower());
        }

        public void AddAccessory(Accessory accessory)
        {
            _context.Accessories.Add(accessory);
        }

        public void UpdateAccessory(Accessory accessory)
        {
            _context.Entry(accessory).State = System.Data.Entity.EntityState.Modified;
        }

        public void DeleteAccessory(int id)
        {
            var accessory = _context.Accessories.Find(id);
            if (accessory != null)
            {
                _context.Accessories.Remove(accessory);
            }
        }

        public bool AccessoryExists(string name)
        {
            return _context.Accessories.Any(a => a.Name.ToLower() == name.ToLower());
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        
        public void SeedAccessories()
        {
            if (!_context.Accessories.Any())
            {
                // Eyewear Cases
                var cases = new List<Accessory>
                {
                    new Accessory { Name = "Premium Hard Shell Eyewear Case", Category = "case", Price = 24.99m, ImagePath = "/Content/Images/Accessories/case1.jpg", Description = "Durable hard shell case with soft interior lining to protect your glasses from scratches and damage. Includes cleaning cloth.", IsActive = true },
                    new Accessory { Name = "Leather Glasses Case", Category = "case", Price = 34.99m, ImagePath = "/Content/Images/Accessories/case2.jpg", Description = "Elegant leather case with magnetic closure. Available in black, brown, and burgundy colors.", IsActive = true },
                    new Accessory { Name = "Slim Soft Pouch", Category = "case", Price = 12.99m, ImagePath = "/Content/Images/Accessories/case3.jpg", Description = "Lightweight microfiber pouch that doubles as a cleaning cloth. Perfect for on-the-go protection.", IsActive = true },
                    new Accessory { Name = "Designer Hard Case", Category = "case", Price = 39.99m, ImagePath = "/Content/Images/Accessories/case4.jpg", Description = "Stylish designer hard case with embossed pattern and premium finish. Makes a great gift.", IsActive = true }
                };

                // Cleaning Sprays
                var sprays = new List<Accessory>
                {
                    new Accessory { Name = "Anti-Fog Lens Spray", Category = "spray", Price = 9.99m, ImagePath = "/Content/Images/Accessories/spray1.jpg", Description = "Prevents fogging on all types of lenses. Perfect for mask wearers and cold weather. 60ml bottle.", IsActive = true },
                    new Accessory { Name = "Premium Lens Cleaner Spray", Category = "spray", Price = 8.99m, ImagePath = "/Content/Images/Accessories/spray2.jpg", Description = "Streak-free formula safe for all lens coatings. Removes fingerprints, oil, and dust effectively. 120ml bottle.", IsActive = true },
                    new Accessory { Name = "Travel Size Cleaning Kit", Category = "spray", Price = 12.99m, ImagePath = "/Content/Images/Accessories/spray3.jpg", Description = "Compact cleaning kit with 30ml spray bottle and microfiber cloth. TSA-approved size for travel.", IsActive = true },
                    new Accessory { Name = "Eco-Friendly Lens Cleaner", Category = "spray", Price = 11.99m, ImagePath = "/Content/Images/Accessories/spray4.jpg", Description = "Plant-based, biodegradable formula in recycled packaging. Gentle yet effective cleaning. 200ml bottle.", IsActive = true }
                };

                // Cleaning Cloths
                var cloths = new List<Accessory>
                {
                    new Accessory { Name = "Microfiber Cleaning Cloth Pack", Category = "cloth", Price = 7.99m, ImagePath = "/Content/Images/Accessories/cloth1.jpg", Description = "Set of 5 ultra-soft microfiber cloths in different colors. Machine washable and lint-free.", IsActive = true },
                    new Accessory { Name = "Premium Chamois Cleaning Cloth", Category = "cloth", Price = 9.99m, ImagePath = "/Content/Images/Accessories/cloth2.jpg", Description = "Natural chamois leather cloth that cleans without scratching. Superior absorption and durability.", IsActive = true },
                    new Accessory { Name = "XL Cleaning Cloth", Category = "cloth", Price = 6.99m, ImagePath = "/Content/Images/Accessories/cloth3.jpg", Description = "Extra large (12\"x12\") microfiber cloth for quick and efficient cleaning of all lens types.", IsActive = true },
                    new Accessory { Name = "Designer Pattern Cloths", Category = "cloth", Price = 14.99m, ImagePath = "/Content/Images/Accessories/cloth4.jpg", Description = "Set of 3 decorative microfiber cloths with stylish patterns. Functional and fashionable.", IsActive = true }
                };

                // Cords & Chains
                var cords = new List<Accessory>
                {
                    new Accessory { Name = "Adjustable Eyewear Retainer", Category = "cord", Price = 8.99m, ImagePath = "/Content/Images/Accessories/cord1.jpg", Description = "Comfortable neoprene strap with adjustable fit. Keeps glasses secure during activities.", IsActive = true },
                    new Accessory { Name = "Metal Chain Strap", Category = "cord", Price = 19.99m, ImagePath = "/Content/Images/Accessories/cord2.jpg", Description = "Elegant gold or silver-toned metal chain with secure silicone ends. Perfect for reading glasses.", IsActive = true },
                    new Accessory { Name = "Sports Glasses Strap", Category = "cord", Price = 12.99m, ImagePath = "/Content/Images/Accessories/cord3.jpg", Description = "High-grip silicone strap designed for athletic activities. Floats in water and includes quick-release feature.", IsActive = true },
                    new Accessory { Name = "Beaded Eyewear Chain", Category = "cord", Price = 24.99m, ImagePath = "/Content/Images/Accessories/cord4.jpg", Description = "Fashionable beaded chain that transforms your glasses into a stylish accessory. Multiple color options available.", IsActive = true }
                };

                // Add all accessories to the database
                _context.Accessories.AddRange(cases);
                _context.Accessories.AddRange(sprays);
                _context.Accessories.AddRange(cloths);
                _context.Accessories.AddRange(cords);
                _context.SaveChanges();

                Console.WriteLine("Default accessories created successfully.");
            }
            else
            {
                Console.WriteLine("Accessories already exist in the database. Skipping seed.");
            }
        }

    }
}