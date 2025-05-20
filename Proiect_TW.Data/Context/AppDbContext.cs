using System.Data.Entity;
using Proiect_TW.Domain.Entities;

namespace Proiect_TW.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection") { }

        public DbSet<User> Users { get; set; }
    }
} 