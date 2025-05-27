using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetAllOrders", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(MapOrderFromReader(reader));
                        }
                    }
                }
            }

            // Load order items for each order
            foreach (var order in orders)
            {
                order.Items = GetOrderItemsByOrderId(order.Id);
            }

            return orders;
        }

        public Order GetOrderById(int id)
        {
            Order order = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetOrderById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = MapOrderFromReader(reader);
                        }
                    }
                }
            }

            if (order != null)
            {
                order.Items = GetOrderItemsByOrderId(order.Id);
            }

            return order;
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetOrdersByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(MapOrderFromReader(reader));
                        }
                    }
                }
            }

            // Load order items for each order
            foreach (var order in orders)
            {
                order.Items = GetOrderItemsByOrderId(order.Id);
            }

            return orders;
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetOrderItemsByOrderId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderItems.Add(MapOrderItemFromReader(reader));
                        }
                    }
                }
            }

            return orderItems;
        }

        public int CreateOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_CreateOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderNumber", SqlDbType.NVarChar, 50).Value = order.OrderNumber;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = order.UserId;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = order.TotalAmount;
                    command.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, 500).Value = (object)order.ShippingAddress ?? DBNull.Value;
                    command.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, 500).Value = (object)order.BillingAddress ?? DBNull.Value;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = (object)order.Phone ?? DBNull.Value;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = order.Email;
                    command.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 50).Value = order.PaymentMethod;
                    command.Parameters.Add("@PaymentStatus", SqlDbType.NVarChar, 50).Value = order.PaymentStatus;
                    command.Parameters.Add("@OrderStatus", SqlDbType.NVarChar, 50).Value = order.OrderStatus;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value = (object)order.Notes ?? DBNull.Value;

                    connection.Open();
                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public int AddOrderItem(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AddOrderItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderItem.OrderId;
                    command.Parameters.Add("@ProductId", SqlDbType.Int).Value = orderItem.ProductId;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = orderItem.Quantity;
                    command.Parameters.Add("@Price", SqlDbType.Decimal).Value = orderItem.Price;
                    command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = orderItem.Discount;

                    connection.Open();
                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public void UpdateOrderStatus(int id, string orderStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdateOrderStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@OrderStatus", SqlDbType.NVarChar, 50).Value = orderStatus;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePaymentStatus(int id, string paymentStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdatePaymentStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@PaymentStatus", SqlDbType.NVarChar, 50).Value = paymentStatus;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddTrackingNumber(int id, string trackingNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AddTrackingNumber", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@TrackingNumber", SqlDbType.NVarChar, 100).Value = trackingNumber;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CancelOrder(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_CancelOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool DeleteOrder(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_DeleteOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }

        public List<Order> GetOrdersByStatus(string orderStatus)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetOrdersByStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderStatus", SqlDbType.NVarChar, 50).Value = orderStatus;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(MapOrderFromReader(reader));
                        }
                    }
                }
            }

            // Load order items for each order
            foreach (var order in orders)
            {
                order.Items = GetOrderItemsByOrderId(order.Id);
            }

            return orders;
        }

        public List<Order> GetRecentOrders(int count = 10)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetRecentOrders", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Count", SqlDbType.Int).Value = count;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(MapOrderFromReader(reader));
                        }
                    }
                }
            }

            // Load order items for each order
            foreach (var order in orders)
            {
                order.Items = GetOrderItemsByOrderId(order.Id);
            }

            return orders;
        }

        public Order GetOrderByOrderNumber(string orderNumber)
        {
            Order order = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetOrderByOrderNumber", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderNumber", SqlDbType.NVarChar, 50).Value = orderNumber;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = MapOrderFromReader(reader);
                        }
                    }
                }
            }

            if (order != null)
            {
                order.Items = GetOrderItemsByOrderId(order.Id);
            }

            return order;
        }

        public void UpdateOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdateOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = order.Id;
                    command.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, 500).Value = (object)order.ShippingAddress ?? DBNull.Value;
                    command.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, 500).Value = (object)order.BillingAddress ?? DBNull.Value;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = (object)order.Phone ?? DBNull.Value;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = order.Email;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value = (object)order.Notes ?? DBNull.Value;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private Order MapOrderFromReader(SqlDataReader reader)
        {
            return new Order
            {
                Id = Convert.ToInt32(reader["Id"]),
                OrderNumber = reader["OrderNumber"].ToString(),
                UserId = Convert.ToInt32(reader["UserId"]),
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                ShippingAddress = reader["ShippingAddress"] != DBNull.Value ? reader["ShippingAddress"].ToString() : null,
                BillingAddress = reader["BillingAddress"] != DBNull.Value ? reader["BillingAddress"].ToString() : null,
                Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                Email = reader["Email"].ToString(),
                PaymentMethod = reader["PaymentMethod"].ToString(),
                PaymentStatus = reader["PaymentStatus"].ToString(),
                OrderStatus = reader["OrderStatus"].ToString(),
                TrackingNumber = reader["TrackingNumber"] != DBNull.Value ? reader["TrackingNumber"].ToString() : null,
                Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null,
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : (DateTime?)null
            };
        }

        private OrderItem MapOrderItemFromReader(SqlDataReader reader)
        {
            return new OrderItem
            {
                Id = Convert.ToInt32(reader["Id"]),
                OrderId = Convert.ToInt32(reader["OrderId"]),
                ProductId = Convert.ToInt32(reader["ProductId"]),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Price = Convert.ToDecimal(reader["Price"]),
                Discount = Convert.ToDecimal(reader["Discount"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                ProductName = reader["ProductName"].ToString(),
                ImageUrl = reader["ImageUrl"].ToString()
            };
        }
    }
}