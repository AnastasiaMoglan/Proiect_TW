using System.Collections.Generic;
using System.Linq;
using eUseControl.Domain.Models;

namespace eUseControl.BusinessLogic.Logic
{
    public class RegistrationBL
    {
        // Simulated user store (shared for registration)
        private static List<Userr> _users = new List<Userr>
        {
            new Userr { Id = 1, Username = "admin", Password = "1234", Email = "admin@example.com", AccessLevel = 1 },
            new Userr { Id = 2, Username = "user", Password = "pass", Email = "user@example.com", AccessLevel = 0 }
        };

        // Register a new user
        public bool Register(RegisterData data, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Password))
            {
                error = "Username, email, and password are required.";
                return false;
            }
            if (_users.Any(u => u.Username == data.Username))
            {
                error = "Username already exists.";
                return false;
            }
            if (_users.Any(u => u.Email == data.Email))
            {
                error = "Email already registered.";
                return false;
            }
            if (data.Password != data.ConfirmPassword)
            {
                error = "Passwords do not match.";
                return false;
            }
            var newUser = new Userr
            {
                Id = _users.Max(u => u.Id) + 1,
                Username = data.Username,
                Password = data.Password,
                Email = data.Email,
                AccessLevel = 0
            };
            _users.Add(newUser);
            return true;
        }

        // For testing: get all users
        public List<Userr> GetAllUsers() => _users;
    }
} 