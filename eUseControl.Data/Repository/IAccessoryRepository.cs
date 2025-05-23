using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Repository
{
    public interface IAccessoryRepository
    {
        List<Accessory> GetAllAccessories();
        List<Accessory> GetAccessoriesByCategory(string category);
        Accessory GetAccessoryById(int id);
        Accessory GetAccessoryByName(string name);
        void AddAccessory(Accessory accessory);
        void UpdateAccessory(Accessory accessory);
        void DeleteAccessory(int id);
        bool AccessoryExists(string name);
        void SaveChanges();
    }
}
