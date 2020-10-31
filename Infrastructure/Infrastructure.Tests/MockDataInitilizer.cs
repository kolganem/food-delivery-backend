using System.IO;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests
{
    public class MockDataInitializer
    {
        public void Seed(FoodDeliveryDbContext context)
        {
            // Create the database if it doesn't exist
            context.Database.EnsureDeleted();

            // Drop the database if it exists
            context.Database.EnsureCreated();

            // read script
            var mockData = File.ReadAllText(@"Data\mockData.sql");

            // execute script
            context.Database.ExecuteSqlRaw(mockData);

            context.SaveChanges();
        }
    }
}
