using System;
using System.Collections.Generic;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public Order GetOrderById(int id)
        {
            return _orderRepository.GetOrderById(id);
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return _orderRepository.GetOrdersByUserId(userId);
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return _orderRepository.GetOrderItemsByOrderId(orderId);
        }

        public int CreateOrder(Order order)
        {
            if (string.IsNullOrEmpty(order.OrderNumber))
            {
                order.OrderNumber = GenerateOrderNumber();
            }
            
            return _orderRepository.CreateOrder(order);
        }

        public int AddOrderItem(OrderItem orderItem)
        {
            return _orderRepository.AddOrderItem(orderItem);
        }

        public void UpdateOrderStatus(int id, string orderStatus)
        {
            _orderRepository.UpdateOrderStatus(id, orderStatus);
        }

        public void UpdatePaymentStatus(int id, string paymentStatus)
        {
            _orderRepository.UpdatePaymentStatus(id, paymentStatus);
        }

        public void AddTrackingNumber(int id, string trackingNumber)
        {
            _orderRepository.AddTrackingNumber(id, trackingNumber);
        }

        public void CancelOrder(int id)
        {
            _orderRepository.CancelOrder(id);
        }

        public bool DeleteOrder(int id)
        {
            return _orderRepository.DeleteOrder(id);
        }

        public List<Order> GetOrdersByStatus(string orderStatus)
        {
            return _orderRepository.GetOrdersByStatus(orderStatus);
        }

        public List<Order> GetRecentOrders(int count = 10)
        {
            return _orderRepository.GetRecentOrders(count);
        }

        public Order GetOrderByOrderNumber(string orderNumber)
        {
            return _orderRepository.GetOrderByOrderNumber(orderNumber);
        }

        public void UpdateOrder(Order order)
        {
            _orderRepository.UpdateOrder(order);
        }

        public string GenerateOrderNumber()
        {
            // Generate order number format: EC-yyyyMMdd-xxxx
            // EC: EyeCare, yyyyMMdd: date, xxxx: random 4-digit number
            return $"EC-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        }
    }
}