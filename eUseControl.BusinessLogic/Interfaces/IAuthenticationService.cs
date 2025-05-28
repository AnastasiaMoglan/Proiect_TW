using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IAuthenticationService
    {
        User ValidateUser(string email, string password);
        bool IsUserLocked(string email);
        void UpdateLoginAttempts(string email, bool successful);
    }
}