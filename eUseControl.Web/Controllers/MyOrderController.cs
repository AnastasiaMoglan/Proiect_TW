using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    [Authorize]
    public class MyOrderController : Controller
    {
        private readonly IOrderService _orderService;

        public MyOrderController(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        // GET: MyOrder
        public ActionResult Index()
        {
            try
            {
                int userId = GetCurrentUserId();
                var orders = _orderService.GetOrdersByUserId(userId);
                
                var viewModel = new MyOrderViewModel
                {
                    Orders = orders.Select(order => new OrderViewModel
                    {
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber,
                        OrderDate = order.OrderDate,
                        OrderStatus = order.OrderStatus,
                        PaymentStatus = order.PaymentStatus,
                        PaymentMethod = order.PaymentMethod,
                        TotalAmount = order.TotalAmount,
                        ShippingAddress = order.ShippingAddress,
                        TrackingNumber = order.TrackingNumber,
                        Items = order.Items.Select(item => new OrderItemViewModel
                        {
                            OrderItemId = item.Id,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            ImageUrl = item.ImageUrl,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            Discount = item.Discount
                        }).ToList()
                    }).ToList()
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load orders: " + ex.Message;
                return View(new MyOrderViewModel());
            }
        }
        
        // GET: MyOrder/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                int userId = GetCurrentUserId();
                var order = _orderService.GetOrderById(id);
                
                if (order == null || order.UserId != userId)
                {
                    return HttpNotFound();
                }
                
                var viewModel = new OrderDetailViewModel
                {
                    Order = new OrderViewModel
                    {
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber,
                        OrderDate = order.OrderDate,
                        OrderStatus = order.OrderStatus,
                        PaymentStatus = order.PaymentStatus,
                        PaymentMethod = order.PaymentMethod,
                        TotalAmount = order.TotalAmount,
                        ShippingAddress = order.ShippingAddress,
                        TrackingNumber = order.TrackingNumber,
                        Items = order.Items.Select(item => new OrderItemViewModel
                        {
                            OrderItemId = item.Id,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            ImageUrl = item.ImageUrl,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            Discount = item.Discount
                        }).ToList()
                    }
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load order details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        // GET: MyOrder/Track
        public ActionResult Track()
        {
            return View(new OrderTrackingViewModel());
        }
        
        // POST: MyOrder/Track
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Track(OrderTrackingViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                
                var order = _orderService.GetOrderByOrderNumber(model.OrderNumber);
                
                if (order == null)
                {
                    ModelState.AddModelError("OrderNumber", "Order not found.");
                    return View(model);
                }
                
                model.Order = new OrderViewModel
                {
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus,
                    PaymentStatus = order.PaymentStatus,
                    PaymentMethod = order.PaymentMethod,
                    TotalAmount = order.TotalAmount,
                    ShippingAddress = order.ShippingAddress,
                    TrackingNumber = order.TrackingNumber
                };
                
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to track order: " + ex.Message);
                return View(model);
            }
        }
        
        // GET: MyOrder/Cancel/5
        public ActionResult Cancel(int id)
        {
            try
            {
                int userId = GetCurrentUserId();
                var order = _orderService.GetOrderById(id);
                
                if (order == null || order.UserId != userId)
                {
                    return HttpNotFound();
                }
                
                if (order.OrderStatus == "Shipped" || order.OrderStatus == "Delivered")
                {
                    TempData["ErrorMessage"] = "Cannot cancel an order that has been shipped or delivered.";
                    return RedirectToAction("Details", new { id });
                }
                
                _orderService.CancelOrder(id);
                TempData["SuccessMessage"] = "Order cancelled successfully.";
                
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to cancel order: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        private int GetCurrentUserId()
        {
            if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out int userId))
            {
                return userId;
            }
            
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims
                    .FirstOrDefault(c => c.Type == "UserId");
                
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out userId))
                {
                    return userId;
                }
            }
            
            // If we can't get the user ID, throw an exception
            throw new InvalidOperationException("User is not authenticated or user ID is not available.");
        }
    }
}