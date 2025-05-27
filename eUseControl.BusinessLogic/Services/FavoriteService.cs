using System;
using System.Collections.Generic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public List<Favorite> GetUserFavorites(int userId)
        {
            return _favoriteRepository.GetAllFavorites(userId);
        }

        public void AddToFavorites(int userId, int productId)
        {
            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = productId,
                DateAdded = DateTime.Now
            };
            _favoriteRepository.AddFavorite(favorite);
            _favoriteRepository.SaveChanges();
        }

        public void RemoveFromFavorites(int userId, int favoriteId)
        {
            _favoriteRepository.RemoveFavorite(favoriteId);
            _favoriteRepository.SaveChanges();
        }

        public void ToggleFavorite(int userId, int productId)
        {
            var favorite = _favoriteRepository.GetFavoriteByUserAndProduct(userId, productId);
            if (favorite != null)
            {
                _favoriteRepository.RemoveFavorite(favorite.Id);
            }
            else
            {
                AddToFavorites(userId, productId);
                return;
            }
            _favoriteRepository.SaveChanges();
        }

        public bool IsFavorite(int userId, int productId)
        {
            return _favoriteRepository.IsFavorite(userId, productId);
        }

        public bool IsProductFavorited(int userId, int productId)
        {
            return _favoriteRepository.IsFavorite(userId, productId);
        }
    }
}