using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Data.Helpers;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;
using eUseControl.Domain.Models;
using LoginRecord = eUseControl.Domain.Entities.LoginRecord;

namespace eUseControl.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        
        public UserRepository()
        {
            _dbContext = new AppDbContext();
        }
        
        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }
        
        public User GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }

        public User GetUserByUsernameOrEmail(string identifier)
        {
            return _dbContext.Users.FirstOrDefault(u => 
                u.Username == identifier || u.Email == identifier);
        }

        public bool IsUserRegistered(string username)
        {
            return _dbContext.Users.Any(u => u.Username == username);
        }
        
        public bool IsEmailRegistered(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }

        public bool UserExists(string username, string email)
        {
            return _dbContext.Users.Any(u => 
                (!string.IsNullOrEmpty(username) && u.Username == username) || 
                (!string.IsNullOrEmpty(email) && u.Email == email));
        }

        public int RegisterUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user.Id;
        }

        public User CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public bool UpdateUser(User user)
        {
            try
            {
                var existingUser = _dbContext.Users.Find(user.Id);
                if (existingUser == null)
                    return false;
                    
                // Update properties
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Phone = user.Phone;
                existingUser.Address = user.Address;
                existingUser.IsActive = user.IsActive;
                existingUser.Role = user.Role;
                
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                var user = _dbContext.Users.Find(userId);
                if (user == null)
                    return false;
                    
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdatePassword(int userId, string newHash, string newSalt)
        {
            var user = _dbContext.Users.Find(userId);
            if (user != null)
            {
                user.PasswordHash = newHash;
                if (user.GetType().GetProperty("Salt") != null) // Check if Salt property exists
                {
                    user.Salt = newSalt;
                }
                _dbContext.SaveChanges();
            }
        }

        public void RecordLoginAttempt(string email, string ip, string userAgent, bool isSuccessful)
        {
            var loginRecord = new LoginRecord
            {
                Email = email,
                IP = ip,
                UserAgent = userAgent,
                IsSuccessful = isSuccessful,
                LoginTime = DateTime.Now
            };
            
            _dbContext.LoginRecords.Add(loginRecord);
            _dbContext.SaveChanges();
            
            // If login was successful, update the user's LastLogin time
            if (isSuccessful)
            {
                var user = GetUserByEmail(email);
                if (user != null)
                {
                    user.LastLogin = DateTime.Now;
                    _dbContext.SaveChanges();
                }
            }
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public void SeedUsers()
        {
            if (!_dbContext.Users.Any())
            {
                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@eyecare.com",
                    PasswordHash = PasswordHasher.HashPassword("Admin123!"),
                    Role = "Admin",
                    CreatedAt = DateTime.Now,
                };

                var standardUser = new User
                {
                    Username = "user",
                    Email = "user@example.com",
                    PasswordHash = PasswordHasher.HashPassword("User123!"),
                    Role = "User", // Standard user level
                    CreatedAt = DateTime.Now,
                };

                _dbContext.Users.Add(adminUser);
                _dbContext.Users.Add(standardUser);
                _dbContext.SaveChanges();

                Console.WriteLine("Default users created successfully.");
            }
            else
            {
                Console.WriteLine("Users already exist in the database. Skipping seed.");
            }
        }

    }
}