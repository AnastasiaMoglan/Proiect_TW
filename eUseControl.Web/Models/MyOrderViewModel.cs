// eUseControl.Web/Models/MyOrderViewModel.cs
using System.Collections.Generic;

namespace eUseControl.Web.Models
{
    public class MyOrderViewModel
    {
        public List<OrderViewModel> Orders { get; set; }

        public MyOrderViewModel()
        {
            Orders = new List<OrderViewModel>();
        }
    }
}