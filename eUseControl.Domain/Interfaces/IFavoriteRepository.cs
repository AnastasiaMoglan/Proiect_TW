using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        List<Favorite> GetAllFavorites(int userId);                    // Obține toate favoritele pentru un utilizator
        Favorite GetFavoriteById(int id);                              // Obține un favorit după ID
        Favorite GetFavoriteByUserAndProduct(int userId, int productId); // Obține un favorit după utilizator și produs (pentru verificări)
        void AddFavorite(Favorite favorite);                           // Adaugă un favorit nou
        void RemoveFavorite(int id);                                   // Șterge favorit după ID
        bool IsFavorite(int userId, int productId);                    // Verifică dacă un produs este favorit pentru utilizator
        void SaveChanges();                                            // Salvează modificările în baza de date
    }
}