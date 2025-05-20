using System.Data.Entity;
using Proiect_TW.Data.Context;
using Proiect_TW.Domain.Entities;

namespace Proiect_TW.Data.Initializer
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            context.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = "123456",
                Email = "admin@example.com"
            });

            base.Seed(context);
        }
    }
} 