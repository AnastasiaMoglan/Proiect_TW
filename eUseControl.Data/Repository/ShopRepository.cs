using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Data.Repository
{
    public class ShopRepository : IShopRepository
    {
        private readonly AppDbContext _db;

        public ShopRepository()
        {
            _db = new AppDbContext();
        }

        public List<Shop> GetAllShops()
        {
            // Returnează doar magazine active
            return _db.Shops.Where(s => s.IsActive).ToList();
        }

        public List<Shop> GetShopsByCity(string city)
        {
            return _db.Shops.Where(s => s.City == city && s.IsActive).ToList();
        }

        public Shop GetShopById(int id)
        {
            return _db.Shops.FirstOrDefault(s => s.Id == id && s.IsActive);
        }

        public List<Shop> GetShopsWithOptician()
        {
            return _db.Shops.Where(s => s.HasOptician && s.IsActive).ToList();
        }

        public List<Shop> GetServiceCenters()
        {
            return _db.Shops.Where(s => s.IsServiceCenter && s.IsActive).ToList();
        }

        public void AddShop(Shop shop)
        {
            shop.CreatedAt = DateTime.Now;
            _db.Shops.Add(shop);
            _db.SaveChanges();
        }

        public void UpdateShop(Shop shop)
        {
            var existingShop = _db.Shops.Find(shop.Id);

            if (existingShop != null)
            {
                existingShop.Name = shop.Name;
                existingShop.Address = shop.Address;
                existingShop.City = shop.City;
                existingShop.PhoneNumber = shop.PhoneNumber;
                existingShop.Email = shop.Email;
                existingShop.Description = shop.Description;
                existingShop.ImageUrl = shop.ImageUrl;
                existingShop.OpeningHours = shop.OpeningHours;
                existingShop.Latitude = shop.Latitude;
                existingShop.Longitude = shop.Longitude;
                existingShop.HasParkingAvailable = shop.HasParkingAvailable;
                existingShop.IsServiceCenter = shop.IsServiceCenter;
                existingShop.HasOptician = shop.HasOptician;
                existingShop.IsActive = shop.IsActive;
                existingShop.UpdatedAt = DateTime.Now;

                _db.Entry(existingShop).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void DeleteShop(int id)
        {
            var shop = _db.Shops.Find(id);

            if (shop != null)
            {
                // Ștergere logică: setează IsActive pe false
                shop.IsActive = false;
                shop.UpdatedAt = DateTime.Now;
                _db.Entry(shop).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void SeedShops()
        {
            if (!_db.Shops.Any())
            {
                var shops = new List<Shop>
                {
                    new Shop
                    {
                        Name = "EyeCare Center Downtown",
                        Address = "123 Main Street",
                        City = "New York",
                        PhoneNumber = "(212) 555-1234",
                        Email = "downtown@eyecare.com",
                        Description = "Our flagship store with full optical services and the latest designer frames.",
                        ImageUrl = "~/Content/Images/Shops/shop1.jpg",
                        OpeningHours = "Mon-Fri: 9AM-7PM, Sat: 10AM-5PM, Sun: Closed",
                        Latitude = 40.712776m,
                        Longitude = -74.005974m,
                        HasParkingAvailable = true,
                        IsServiceCenter = true,
                        HasOptician = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    // ... alte magazine identic cu cele din codul tău ...
                };

                _db.Shops.AddRange(shops);
                _db.SaveChanges();
            }
        }
    }
}
