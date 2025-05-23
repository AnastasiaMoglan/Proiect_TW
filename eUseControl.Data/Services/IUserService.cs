using eUseControl.Domain.Entities;
using System.Collections.Generic;

namespace eUseControl.Data.Services
{
    public interface IUserService
    {
        bool IsUserRegistered(string username);
        bool IsEmailRegistered(string email);
        User RegisterUser(string email, string username, string password);
        User ValidateUser(string email, string password);
    }
}