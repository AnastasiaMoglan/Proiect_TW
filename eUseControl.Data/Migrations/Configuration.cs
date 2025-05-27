using System.Data.Entity.Migrations;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Models;
using System.Linq;
using eUseControl.Data.Context;

namespace eUseControl.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppDbContext context)
        {
            // Add a test user if the Users table is empty
            if (!context.Users.Any())
            {
                context.Users.Add(new User 
                { 
                    Username = "Test User",
                    Email = "test@example.com",
                    PasswordHash = "password123", // In real application, this should be hashed
                });
                
                context.SaveChanges();
            }
            
        }
    }
} 