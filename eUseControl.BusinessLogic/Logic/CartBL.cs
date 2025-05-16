using System.Collections.Generic;
using System.Linq;
using eUseControl.Domain.Models;

namespace eUseControl.BusinessLogic.Logic
{
    public class CartBL
    {
        public class CartItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        // Simulated cart store: userId -> list of CartItems
        private static Dictionary<int, List<CartItem>> _carts = new Dictionary<int, List<CartItem>>();

        // Add a product to user's cart (or increase quantity)
        public void AddToCart(int userId, int productId, int quantity = 1)
        {
            if (!_carts.ContainsKey(userId))
                _carts[userId] = new List<CartItem>();
            var item = _carts[userId].FirstOrDefault(ci => ci.ProductId == productId);
            if (item == null)
                _carts[userId].Add(new CartItem { ProductId = productId, Quantity = quantity });
            else
                item.Quantity += quantity;
        }

        // Remove a product from user's cart
        public void RemoveFromCart(int userId, int productId)
        {
            if (_carts.ContainsKey(userId))
                _carts[userId].RemoveAll(ci => ci.ProductId == productId);
        }

        // Get all cart items for a user
        public List<CartItem> GetCart(int userId)
        {
            if (_carts.ContainsKey(userId))
                return _carts[userId];
            return new List<CartItem>();
        }

        // Clear the cart for a user
        public void ClearCart(int userId)
        {
            if (_carts.ContainsKey(userId))
                _carts[userId].Clear();
        }
    }
} 