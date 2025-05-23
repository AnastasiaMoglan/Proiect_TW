using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Repository
{
    public interface IShopRepository
    {
        List<Shop> GetAllShops();
        List<Shop> GetShopsByCity(string city);
        Shop GetShopById(int id);
        List<Shop> GetShopsWithOptician();
        List<Shop> GetServiceCenters();
        void AddShop(Shop shop);
        void UpdateShop(Shop shop);
        void DeleteShop(int id);
        void SeedShops();
    }
}