using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Data.Helpers;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Models;

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
        
        public bool IsUserRegistered(string username)
        {
            return _dbContext.Users.Any(u => u.Username == username);
        }
        
        public bool IsEmailRegistered(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }
        
        public User CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
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