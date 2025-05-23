using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;

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
                        HasOptician = true
                    },
                    new Shop
                    {
                        Name = "EyeCare Express Midtown", 
                        Address = "456 Park Avenue", 
                        City = "New York", 
                        PhoneNumber = "(212) 555-5678", 
                        Email = "midtown@eyecare.com", 
                        Description = "Express service for busy professionals. Get your glasses in under an hour!", 
                        ImageUrl = "~/Content/Images/Shops/shop2.jpg", 
                        OpeningHours = "Mon-Fri: 8AM-8PM, Sat-Sun: 10AM-6PM", 
                        Latitude = 40.754932m, 
                        Longitude = -73.984016m, 
                        HasParkingAvailable = false, 
                        IsServiceCenter = true, 
                        HasOptician = true
                    },
                    new Shop
                    {
                        Name = "EyeCare Luxury Collection", 
                        Address = "789 Fifth Avenue", 
                        City = "New York", 
                        PhoneNumber = "(212) 555-9012", 
                        Email = "luxury@eyecare.com", 
                        Description = "Premium designer eyewear and personalized styling consultations.", 
                        ImageUrl = "~/Content/Images/Shops/shop3.jpg", 
                        OpeningHours = "Mon-Sat: 10AM-7PM, Sun: 12PM-5PM", 
                        Latitude = 40.763841m, 
                        Longitude = -73.973388m, 
                        HasParkingAvailable = true, 
                        IsServiceCenter = false, 
                        HasOptician = true
                    },
                    new Shop
                    {
                        Name = "EyeCare Family Center", 
                        Address = "321 Maple Street", 
                        City = "Brooklyn", 
                        PhoneNumber = "(718) 555-3456", 
                        Email = "brooklyn@eyecare.com", 
                        Description = "Family-friendly store with specialized services for children and teens.", 
                        ImageUrl = "~/Content/Images/Shops/shop4.jpg", 
                        OpeningHours = "Mon-Fri: 9AM-6PM, Sat: 9AM-5PM, Sun: Closed", 
                        Latitude = 40.650002m, 
                        Longitude = -73.949997m, 
                        HasParkingAvailable = true, 
                        IsServiceCenter = true, 
                        HasOptician = true
                    },
                    new Shop
                    {
                        Name = "EyeCare Quick Service", 
                        Address = "654 Broadway", 
                        City = "Queens", 
                        PhoneNumber = "(718) 555-7890", 
                        Email = "queens@eyecare.com", 
                        Description = "Rapid optical services with affordable options for the whole family.", 
                        ImageUrl = "~/Content/Images/Shops/shop5.jpg", 
                        OpeningHours = "Mon-Sun: 10AM-9PM", 
                        Latitude = 40.742054m, 
                        Longitude = -73.769417m, 
                        HasParkingAvailable = true, 
                        IsServiceCenter = false, 
                        HasOptician = true
                    },
                    new Shop
                    {
                        Name = "EyeCare Contact Lens Specialist", 
                        Address = "987 Hudson Street", 
                        City = "Jersey City", 
                        PhoneNumber = "(201) 555-2345", 
                        Email = "contacts@eyecare.com", 
                        Description = "Specializing in all types of contact lenses with expert fitting services.", 
                        ImageUrl = "~/Content/Images/Shops/shop6.jpg", 
                        OpeningHours = "Mon-Fri: 9AM-7PM, Sat: 10AM-4PM, Sun: Closed", 
                        Latitude = 40.728157m, 
                        Longitude = -74.077642m, 
                        HasParkingAvailable = false, 
                        IsServiceCenter = false, 
                        HasOptician = true
                    }
                };

                _db.Shops.AddRange(shops);
                _db.SaveChanges();
            }
        }
    }
}