using System.Collections.Generic;
using System.Web.Mvc;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class MyOrderController : Controller
    {
        // GET: MyOrder
        public ActionResult Index()
        {
            // In a real application, you would retrieve the actual orders from a database
            var model = new MyOrderViewModel
            {
                Orders = GetSampleOrders()
            };
            
            return View(model);
        }
        
        // GET: MyOrder/Details/5
        public ActionResult Details(int id)
        {
            // In a real application, you would fetch the specific order details from a database
            var orders = GetSampleOrders();
            var orderDetail = orders.Find(o => o.OrderId == id);
            
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            
            return View(orderDetail);
        }
        
        // Helper method to generate sample data
        private List<Order> GetSampleOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    OrderId = 1001,
                    OrderDate = System.DateTime.Now.AddDays(-5),
                    OrderStatus = "Delivered",
                    TotalAmount = 149.99m,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { ProductId = 101, ProductName = "Contact Lenses - Monthly", Quantity = 2, Price = 49.99m },
                        new OrderItem { ProductId = 203, ProductName = "Lens Solution", Quantity = 1, Price = 50.01m }
                    }
                },
                new Order
                {
                    OrderId = 1002,
                    OrderDate = System.DateTime.Now.AddDays(-2),
                    OrderStatus = "Processing",
                    TotalAmount = 210.50m,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { ProductId = 305, ProductName = "Designer Frames", Quantity = 1, Price = 180.50m },
                        new OrderItem { ProductId = 402, ProductName = "Glasses Case", Quantity = 1, Price = 30.00m }
                    }
                }
            };
        }
    }
}