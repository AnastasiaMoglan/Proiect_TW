using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Data.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _db;

        public FavoriteRepository()
        {
            _db = new AppDbContext();
        }

        public List<Favorite> GetAllFavorites(int userId)
        {
            return _db.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .ToList();
        }

        public Favorite GetFavoriteById(int id)
        {
            return _db.Favorites
                .Include(f => f.Product)
                .FirstOrDefault(f => f.Id == id);
        }

        public Favorite GetFavoriteByUserAndProduct(int userId, int productId)
        {
            return _db.Favorites
                .Include(f => f.Product)
                .FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
        }

        public void AddFavorite(Favorite favorite)
        {
            var existing = GetFavoriteByUserAndProduct(favorite.UserId, favorite.ProductId);
            if (existing == null)
            {
                _db.Favorites.Add(favorite);
                _db.SaveChanges();
            }
        }

        public void RemoveFavorite(int id)
        {
            var favorite = _db.Favorites.Find(id);
            if (favorite != null)
            {
                _db.Favorites.Remove(favorite);
                _db.SaveChanges();
            }
        }

        public bool IsFavorite(int userId, int productId)
        {
            return _db.Favorites.Any(f => f.UserId == userId && f.ProductId == productId);
        }

        // IMPLEMENTARE LIPSA
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}