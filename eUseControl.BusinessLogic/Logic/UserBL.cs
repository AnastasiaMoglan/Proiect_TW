using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Logic
{
    public class UserBL
    {
        // Simulated user store (in a real app, this would come from a database)
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "admin", PasswordHash = "1234", Email = "admin@example.com", Role = "Admin", CreatedAt = DateTime.Now },
            new User { Id = 2, Username = "user", PasswordHash = "pass", Email = "user@example.com", Role = "User", CreatedAt = DateTime.Now }
        };

        public User Login(string username, string password)
        {
            // Check for a user with matching username and password
            return _users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
        }
    }
} 