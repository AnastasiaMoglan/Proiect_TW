using System.Collections.Generic;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        // Constructor cu injecție de dependență
        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public List<Shop> GetAllShops()
        {
            return _shopRepository.GetAllShops();
        }

        public List<Shop> GetShopsByCity(string city)
        {
            return _shopRepository.GetShopsByCity(city);
        }

        public Shop GetShopById(int id)
        {
            return _shopRepository.GetShopById(id);
        }

        public List<Shop> GetShopsWithOptician()
        {
            return _shopRepository.GetShopsWithOptician();
        }

        public List<Shop> GetServiceCenters()
        {
            return _shopRepository.GetServiceCenters();
        }

        public void AddShop(Shop shop)
        {
            _shopRepository.AddShop(shop);
        }

        public void UpdateShop(Shop shop)
        {
            _shopRepository.UpdateShop(shop);
        }

        public void DeleteShop(int id)
        {
            _shopRepository.DeleteShop(id);
        }

        public void SeedShops()
        {
            _shopRepository.SeedShops();
        }
    }
}