using System.Data.Entity;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Models;

namespace eUseControl.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection")
        {
            // Create database if it doesn't exist
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginRecord> LoginRecords { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        
        public virtual DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // User entity configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Configure email uniqueness
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure username uniqueness
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();
                
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // LoginRecord entity configuration
            modelBuilder.Entity<LoginRecord>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<LoginRecord>()
                .Property(l => l.Email)
                .HasMaxLength(100)
                .IsRequired();
                
            // Accessory entity configuration
            modelBuilder.Entity<Accessory>()
                .HasKey(a => a.Id);
                
            modelBuilder.Entity<Accessory>()
                .Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();
                
            modelBuilder.Entity<Accessory>()
                .Property(a => a.Category)
                .HasMaxLength(50)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}