using System.Collections.Generic;
using eUseControl.Domain.Entities;
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Services
{
    public interface IAccessoryService
    {
        List<Accessory> GetAllAccessories();
        List<Accessory> GetAccessoriesByCategory(string category);
        Accessory GetAccessoryById(int id);
        Accessory GetAccessoryByName(string name);
        void AddAccessory(string name, string category, decimal price, string imagePath, string description = null);
        void UpdateAccessory(int id, string name, string category, decimal price, string imagePath, string description = null);
        void DeleteAccessory(int id);
        void SeedAccessories();
        Dictionary<string, List<Accessory>> GetAccessoriesByCategories();
    }
}
