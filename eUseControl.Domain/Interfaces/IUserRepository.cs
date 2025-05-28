using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Domain.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        User GetUserById(int id);
        User GetUserByUsernameOrEmail(string identifier);

        bool IsUserRegistered(string username);
        bool IsEmailRegistered(string email);
        bool UserExists(string username, string email);

        int RegisterUser(User user);
        User CreateUser(User user);

        bool UpdateUser(User user);
        void Update(User user); // ✅ NEW method added — required by AuthenticationService

        bool DeleteUser(int userId);
        void UpdatePassword(int userId, string newHash, string newSalt);
        void RecordLoginAttempt(string email, string ip, string userAgent, bool isSuccessful);

        List<User> GetAllUsers();
    }
}