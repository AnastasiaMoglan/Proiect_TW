using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Models;

namespace eUseControl.BusinessLogic.Logic
{
    public class RegistrationBL
    {
        // Simulated user store (shared for registration)
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "admin", PasswordHash = "1234", Email = "admin@example.com", Role = "Admin", CreatedAt = DateTime.Now },
            new User { Id = 2, Username = "user", PasswordHash = "pass", Email = "user@example.com", Role = "User", CreatedAt = DateTime.Now }
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
            var newUser = new User
            {
                Id = _users.Max(u => u.Id) + 1,
                Username = data.Username,
                PasswordHash = data.Password,
                Email = data.Email,
                Role = "User",
                CreatedAt = DateTime.Now
            };
            _users.Add(newUser);
            return true;
        }

        // For testing: get all users
        public List<User> GetAllUsers() => _users;
    }
} 