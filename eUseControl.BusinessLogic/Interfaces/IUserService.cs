using eUseControl.Domain.Entities;
using System.Collections.Generic;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        bool IsUserRegistered(string username);
        bool IsEmailRegistered(string email);

        // Metodă pentru verificarea existenței userului după username și email
        bool UserExists(string username, string email);

        // Înregistrează user, întoarce bool
        User RegisterUser(string email, string username, string password);

        // Verifică datele de autentificare
        User ValidateUser(string email, string password);

        User GetUserById(int id);

        User GetUserByEmail(string email);

        bool UpdateUser(User user);

        bool DeleteUser(int userId);

        List<User> GetAllUsers();
    }
}