using System;
using System.Data.Entity;
using eUseControl.Data.Context;
using eUseControl.Data.Repository;

namespace eUseControl.Data.Initializer
{
    public class DbInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            try
            {
                Console.WriteLine("Initializing database with seed data...");

                // Seed Users
                Console.WriteLine("Seeding users...");
                var userRepository = new UserRepository();
                userRepository.SeedUsers();
                
                // Seed Accessories
                Console.WriteLine("Seeding accessories...");
                var accessoryRepository = new AccessoryRepository(context);
                accessoryRepository.SeedAccessories();
                
                // Seed Products
                Console.WriteLine("Seeding products...");
                var productRepository = new ProductRepository();
                productRepository.SeedProducts();
                
                // Seed Shops
                Console.WriteLine("Seeding shops...");
                var shopRepository = new ShopRepository();
                shopRepository.SeedShops();
                
                Console.WriteLine("Database seeding completed successfully.");
                
                base.Seed(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DB Initialization: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error in DB Initialization: " + ex.ToString());
            }
        }
    }
}