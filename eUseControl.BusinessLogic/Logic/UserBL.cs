using System.Collections.Generic;
using System.Linq;
using eUseControl.Domain.Models;

namespace eUseControl.BusinessLogic.Logic
{
    public class UserBL
    {
        // Simulated user store (in a real app, this would come from a database)
        private static List<Userr> _users = new List<Userr>
        {
            new Userr { Id = 1, Username = "admin", Password = "1234", Email = "admin@example.com", AccessLevel = 1 },
            new Userr { Id = 2, Username = "user", Password = "pass", Email = "user@example.com", AccessLevel = 0 }
        };

        public Userr Login(string username, string password)
        {
            // Check for a user with matching username and password
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
} 