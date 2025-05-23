using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Models
{
    public class AccessoriesViewModel
    {
        public Dictionary<string, List<Accessory>> AccessoriesByCategory { get; set; }
        
        public List<Accessory> FeaturedAccessories { get; set; }
        
        public AccessoriesViewModel()
        {
            AccessoriesByCategory = new Dictionary<string, List<Accessory>>();
            FeaturedAccessories = new List<Accessory>();
        }
    }
}
