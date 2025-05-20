using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Services
{
    public class UserService
    {
        public bool IsUserRegistered(string username)
        {
            using (var db = new AppDbContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }
    }
} 