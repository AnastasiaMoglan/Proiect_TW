using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Data.Context;
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
        
        public void SaveLoginRecord(LoginRecord loginRecord)
        {
            _dbContext.LoginRecords.Add(loginRecord);
            _dbContext.SaveChanges();
        }
        
        public List<LoginRecord> GetLoginHistory()
        {
            return _dbContext.LoginRecords
                .OrderByDescending(l => l.LoginTime)
                .ToList();
        }
    }
}