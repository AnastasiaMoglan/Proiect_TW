using System.Data.Entity.Migrations;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Models;
using eUseControl.Web.Data;
using System.Linq;

namespace eUseControl.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
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

            // Add a test transfer if the TransferCards table is empty
            if (!context.TransferCards.Any())
            {
                context.TransferCards.Add(new TransferCard
                {
                    BankSender = "Maib",
                    SenderName = "Test User",
                    CardSender = "1234 5678 9012 3456",
                    Amount = 100.00m,
                    CardBeneficiary = "9876 5432 1098 7654",
                    BeneficiaryName = "Test User",
                    BankBeneficiary = "Maib",
                    TransferDate = System.DateTime.Now
                });

                context.SaveChanges();
            }
        }
    }
} 