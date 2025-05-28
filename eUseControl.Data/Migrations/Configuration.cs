using System;
using System.Data.Entity.Migrations;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;

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
           
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Username = "TestUser",
                    Email = "test@example.com",
                    PasswordHash = "hashed_password_123", // În aplicații reale, folosește hash real
                    Salt = "random_salt_here",            // Poți genera random sau folosi unul fix pentru test
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    LoginAttempts = 0,
                    LastLogin = null
                });

                context.SaveChanges();
            }
        }
    }
}