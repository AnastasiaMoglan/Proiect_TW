using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Services
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        List<Order> GetOrdersByUserId(int userId);
        List<OrderItem> GetOrderItemsByOrderId(int orderId);
        int CreateOrder(Order order);
        int AddOrderItem(OrderItem orderItem);
        void UpdateOrderStatus(int id, string orderStatus);
        void UpdatePaymentStatus(int id, string paymentStatus);
        void AddTrackingNumber(int id, string trackingNumber);
        void CancelOrder(int id);
        bool DeleteOrder(int id);
        List<Order> GetOrdersByStatus(string orderStatus);
        List<Order> GetRecentOrders(int count = 10);
        Order GetOrderByOrderNumber(string orderNumber);
        void UpdateOrder(Order order);
        string GenerateOrderNumber();
    }
}