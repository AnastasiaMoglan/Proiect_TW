using System.Collections.Generic;

namespace eUseControl.Web.Models
{
    public class AdminOrdersViewModel
    {
        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
        public string CurrentStatus { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}