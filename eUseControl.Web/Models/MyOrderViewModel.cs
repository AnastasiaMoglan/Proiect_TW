using System;
using System.Collections.Generic;

namespace eUseControl.Web.Models
{
    public class MyOrderViewModel
    {
        public List<Order> Orders { get; set; }

        public MyOrderViewModel()
        {
            Orders = new List<Order>();
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItem> Items { get; set; }
        
        public Order()
        {
            Items = new List<OrderItem>();
        }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * Price;
    }
}