using System.Data.Entity;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Models;
using LoginRecord = eUseControl.Domain.Entities.LoginRecord;

namespace eUseControl.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbSet<MedicalConsultation> contact) : base("name=DefaultConnection")
        {
            Contact = contact;
        }

        public AppDbContext()
        {
            throw new System.NotImplementedException();
        }

        // DbSet properties – defines which tables you map
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<LoginRecord> LoginRecords { get; set; }
        public virtual DbSet<Accessory> Accessories { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        
        public virtual DbSet<TransferCard> TransferCards { get; set; }
        public DbSet<MedicalConsultation> MedicalConsultation { get; set; }
        
        public DbSet<MedicalConsultation> Contact { get; set; }

        public DbSet<TeamMember> TeamMember { get; set; }


    }
}