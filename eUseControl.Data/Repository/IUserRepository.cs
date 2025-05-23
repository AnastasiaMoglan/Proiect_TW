using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Repository
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        User GetUserById(int id);
        bool IsUserRegistered(string username);
        bool IsEmailRegistered(string email);
        User CreateUser(User user);
        void SaveLoginRecord(Domain.Models.LoginRecord loginRecord);
        List<Domain.Models.LoginRecord> GetLoginHistory();
    }
}