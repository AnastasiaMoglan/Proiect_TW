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
    }
}