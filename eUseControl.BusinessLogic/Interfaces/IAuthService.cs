using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Services
{
    public interface IAuthService
    {
        User GetUserById(int id);
        User GetUserByUsernameOrEmail(string identifier);
        bool ValidateCredentials(string usernameOrEmail, string password);
        int RegisterUser(string username, string email, string password, string firstName = null, string lastName = null);
        void UpdateUserProfile(int userId, string firstName, string lastName, string phone, string address);
        void ChangePassword(int userId, string currentPassword, string newPassword);
        void RecordLoginAttempt(string email, string ip, string userAgent, bool isSuccessful);
        bool UsernameExists(string username);
        bool EmailExists(string email);
    }
}