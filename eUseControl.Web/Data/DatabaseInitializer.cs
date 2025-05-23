using System.Data.Entity;
using eUseControl.Domain.Entities;
using System.Linq;

namespace eUseControl.Web.Data
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
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
            
            base.Seed(context);
        }
    }
}