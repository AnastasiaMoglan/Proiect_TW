// File: eUseControl.Web/Controllers/AdminOrderController.cs
using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;
using Unity.Injection;

namespace eUseControl.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly IOrderService _orderService;

        public AdminOrderController(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        public ActionResult Index(string status = null)
        {
            try
            {
                var orders = string.IsNullOrEmpty(status) 
                    ? _orderService.GetAllOrders()
                    : _orderService.GetOrdersByStatus(status);
                
                var viewModel = new AdminOrdersViewModel
                {
                    Orders = orders.Select(o => new OrderViewModel
                    {
                        OrderId = o.Id,
                        OrderNumber = o.OrderNumber,
                        OrderDate = o.OrderDate,
                        OrderStatus = o.OrderStatus,
                        PaymentStatus = o.PaymentStatus,
                        PaymentMethod = o.PaymentMethod,
                        TotalAmount = o.TotalAmount,
                        ShippingAddress = o.ShippingAddress,
                        TrackingNumber = o.TrackingNumber
                    }).ToList(),
                    CurrentStatus = status,
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading orders.";
                ViewBag.DetailedError = ex.Message;
                return View(new AdminOrdersViewModel());
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                Order order = _orderService.GetOrderById(id);
                
                if (order == null)
                    return RedirectToAction("Index");
                
                var viewModel = new AdminOrderDetailViewModel
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
                        Items = order.Items.Select(i => new OrderItemViewModel
                        {
                            OrderItemId = i.Id,
                            ProductId = i.ProductId,
                            ProductName = i.ProductName,
                            ImageUrl = i.ImageUrl,
                            Quantity = i.Quantity,
                            Price = i.Price,
                            Discount = i.Discount
                        }).ToList()
                    }
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading order details.";
                ViewBag.DetailedError = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Update(int id)
        {
            try
            {
                var order = _orderService.GetOrderById(id);
                
                if (order == null)
                    return RedirectToAction("Index");
                
                var viewModel = new OrderUpdateViewModel
                {
                    OrderId = order.Id,
                    OrderStatus = order.OrderStatus,
                    PaymentStatus = order.PaymentStatus,
                    TrackingNumber = order.TrackingNumber
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(OrderUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                
                var order = _orderService.GetOrderById(model.OrderId);
                
                if (order == null)
                    return RedirectToAction("Index");
                
                // Update order status
                _orderService.UpdateOrderStatus(model.OrderId, model.OrderStatus);
                
                TempData["SuccessMessage"] = "Order updated successfully.";
                return RedirectToAction("Details", new { id = model.OrderId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                bool success = _orderService.DeleteOrder(id);
                
                if (success)
                {
                    TempData["SuccessMessage"] = "Order deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the order.";
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}