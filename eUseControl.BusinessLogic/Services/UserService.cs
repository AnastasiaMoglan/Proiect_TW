using System;
using System.Collections.Generic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Helpers;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public bool IsUserRegistered(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));

            return _userRepository.IsUserRegistered(username);
        }

        public bool IsEmailRegistered(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            return _userRepository.IsEmailRegistered(email);
        }

        public bool UserExists(string username, string email)
        {
            return _userRepository.IsUserRegistered(username) || _userRepository.IsEmailRegistered(email);
        }

        public User RegisterUser(string email, string username, string password)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Username is required.", nameof(username));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password is required.", nameof(password));

            if (UserExists(username, email))
            {
                return GetUserByEmail(email);
            }

            var result = PasswordHasher.HashPassword(password);
            var hash = result.Hash;
            var salt = result.Salt;

            var user = new User
            {
                Email = email,
                Username = username,
                PasswordHash = hash,
                Salt = salt,
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var createdUser = _userRepository.CreateUser(user);
            return createdUser;
        }

        public int RegisterUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userRepository.RegisterUser(user);
        }

        public User ValidateUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
                return null;

            if (PasswordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
                return user;

            return null;
        }

        public User GetUserById(int id)
        {
            if (id <= 0) throw new ArgumentException("User ID must be positive.", nameof(id));
            return _userRepository.GetUserById(id);
        }

        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required.", nameof(email));
            return _userRepository.GetUserByEmail(email);
        }

        public bool UpdateUser(User updatedUser)
        {
            if (updatedUser == null) throw new ArgumentNullException(nameof(updatedUser));
            return _userRepository.UpdateUser(updatedUser);
        }

        public bool DeleteUser(int userId)
        {
            if (userId <= 0) throw new ArgumentException("User ID must be positive.", nameof(userId));
            return _userRepository.DeleteUser(userId);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
    }
}
