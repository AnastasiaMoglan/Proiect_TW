using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Services
{
    public interface IAccessoryService
    {
        List<Accessory> GetAllAccessories();
        List<Accessory> GetAccessoriesByCategory(string category);
        Accessory GetAccessoryById(int id);
        Accessory GetAccessoryByName(string name);
        void AddAccessory(Accessory accessory);
        void UpdateAccessory(Accessory accessory);
        void DeleteAccessory(int id);
        void SeedAccessories();
        Dictionary<string, List<Accessory>> GetAccessoriesByCategories();
    }
}