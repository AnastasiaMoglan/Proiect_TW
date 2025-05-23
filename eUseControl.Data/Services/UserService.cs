using System;
using System.Collections.Generic;
using eUseControl.Data.Context;
using eUseControl.Data.Repository;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Models;
using eUseControl.Data.Helpers;

namespace eUseControl.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        
        public UserService()
        {
            _userRepository = new UserRepository();
        }
        
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public bool IsUserRegistered(string username)
        {
            return _userRepository.IsUserRegistered(username);
        }
        
        public bool IsEmailRegistered(string email)
        {
            return _userRepository.IsEmailRegistered(email);
        }
        
        public User RegisterUser(string email, string username, string password)
        {
            if (IsEmailRegistered(email))
                return null;
                
            if (IsUserRegistered(username))
                return null;
                
            var user = new User
            {
                Email = email,
                Username = username,
                PasswordHash = PasswordHasher.HashPassword(password),
                Role = "User",
                CreatedAt = DateTime.Now
            };
            
            return _userRepository.CreateUser(user);
        }
        
        public User ValidateUser(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);
            
            if (user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }
            
            return null;
        }
    }
}