using System.Collections.Generic;
using eUseControl.Data.Repository;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService()
        {
            _shopRepository = new ShopRepository();
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