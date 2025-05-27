using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Services
{
    public interface IFavoriteService
    {
        List<Favorite> GetUserFavorites(int userId);                 // Obține toate favoritele utilizatorului
        void AddToFavorites(int userId, int productId);              // Adaugă un produs la favorite
        void RemoveFromFavorites(int userId, int favoriteId);        // Șterge un favorit
        void ToggleFavorite(int userId, int productId);              // Adaugă sau elimină favorit (toggle)
        bool IsFavorite(int userId, int productId);                  // Verifică dacă produsul este favorit
    }
}