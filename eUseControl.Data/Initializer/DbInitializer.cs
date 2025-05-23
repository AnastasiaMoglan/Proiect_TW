using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Data.Helpers;
using eUseControl.Data.Repository;
using eUseControl.Domain.Entities;

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
                var accessoryRepository = new AccessoryRepository();
                accessoryRepository.SeedAccessories();
                
                // Seed Products
                Console.WriteLine("Seeding products...");
                var productRepository = new ProductRepository();
                productRepository.SeedProducts();
                
                Console.WriteLine("Database seeding completed successfully.");
                
                base.Seed(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DB Initialization: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error in DB Initialization: " + ex.ToString());
                // Log the exception, but don't rethrow to allow the application to continue
                // In a production environment, you might want to implement more robust error handling
            }
        }
    }
}
