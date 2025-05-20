using System.Data.Entity;
using eUseControl.Domain.Entities;

namespace Proiect_TW.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection") { }

        public DbSet<User> Users { get; set; }
    }
} 