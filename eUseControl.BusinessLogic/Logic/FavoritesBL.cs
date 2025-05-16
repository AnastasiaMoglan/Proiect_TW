using System.Collections.Generic;
using System.Linq;

namespace eUseControl.BusinessLogic.Logic
{
    public class FavoritesBL
    {
        // Simulated favorites store: userId -> list of productIds
        private static Dictionary<int, List<int>> _favorites = new Dictionary<int, List<int>>();

        // Add a product to user's favorites
        public void AddToFavorites(int userId, int productId)
        {
            if (!_favorites.ContainsKey(userId))
                _favorites[userId] = new List<int>();
            if (!_favorites[userId].Contains(productId))
                _favorites[userId].Add(productId);
        }

        // Remove a product from user's favorites
        public void RemoveFromFavorites(int userId, int productId)
        {
            if (_favorites.ContainsKey(userId))
                _favorites[userId].Remove(productId);
        }

        // Get all favorite product IDs for a user
        public List<int> GetFavorites(int userId)
        {
            if (_favorites.ContainsKey(userId))
                return _favorites[userId];
            return new List<int>();
        }
    }
} 