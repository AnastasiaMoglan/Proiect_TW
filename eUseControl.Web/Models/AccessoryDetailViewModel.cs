using eUseControl.Domain.Entities;
using System.Collections.Generic;

namespace eUseControl.Web.Models
{
    public class AccessoryDetailViewModel
    {
        public Accessory Accessory { get; set; }
        
        public List<Accessory> RelatedAccessories { get; set; }
        
        public AccessoryDetailViewModel()
        {
            RelatedAccessories = new List<Accessory>();
        }
    }
}
