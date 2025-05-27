using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Data.Repository
{
    public class AccessoryRepository : IAccessoryRepository
    {
        private readonly AppDbContext _context;

        // Constructor cu injectare de dependință AppDbContext
        public AccessoryRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Accessory> GetAllAccessories()
        {
            return _context.Accessories.Where(a => a.IsActive).ToList();
        }

        public List<Accessory> GetAccessoriesByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return new List<Accessory>();

            return _context.Accessories
                .Where(a => a.IsActive && a.Category != null && 
                            a.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Accessory GetAccessoryById(int id)
        {
            return _context.Accessories.FirstOrDefault(a => a.Id == id && a.IsActive);
        }

        public Accessory GetAccessoryByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return _context.Accessories
                .FirstOrDefault(a => a.IsActive && a.Name != null && 
                                     a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void AddAccessory(Accessory accessory)
        {
            if (accessory == null)
                throw new ArgumentNullException(nameof(accessory));

            accessory.CreatedAt = DateTime.UtcNow;
            accessory.IsActive = true;

            _context.Accessories.Add(accessory);
            _context.SaveChanges();
        }

        public void UpdateAccessory(Accessory accessory)
        {
            if (accessory == null)
                throw new ArgumentNullException(nameof(accessory));

            var existing = _context.Accessories.Find(accessory.Id);
            if (existing == null || !existing.IsActive)
                throw new InvalidOperationException("Accessory not found or inactive.");

            existing.Name = accessory.Name;
            existing.Category = accessory.Category;
            existing.Price = accessory.Price;
            existing.ImagePath = accessory.ImagePath;
            existing.Description = accessory.Description;
            existing.IsActive = accessory.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void DeleteAccessory(int id)
        {
            var accessory = _context.Accessories.Find(id);
            if (accessory == null || !accessory.IsActive)
                throw new InvalidOperationException("Accessory not found or already inactive.");

            // Ștergere logică
            accessory.IsActive = false;
            accessory.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public bool AccessoryExists(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return _context.Accessories
                .Any(a => a.IsActive && a.Name != null && 
                          a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void SeedAccessories()
        {
            if (_context.Accessories.Any(a => a.IsActive)) return;

            // Definirea obiectelor de seed (exemplu, adaptează după necesități)
            var accessories = new List<Accessory>
            {
                // ... completează cu accesorii (exemplu din codul tău anterior)
            };

            _context.Accessories.AddRange(accessories);
            _context.SaveChanges();
        }
    }
}
