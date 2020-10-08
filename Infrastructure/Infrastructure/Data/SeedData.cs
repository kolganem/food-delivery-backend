using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Domain.Model;
using System.Collections.Generic;
using System;

namespace Infrastructure
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            FoodDeliveryDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<FoodDeliveryDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }            
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Tea",
                        Description = "Cup of green tea ",
                        Price = 5
                    },
                    new Product
                    {
                        Name = "Coffee",
                        Description = "Cup of pure coffee",
                        Price = 8
                    },
                    new Product
                    {
                        Name = "Milk",
                        Description = "Bottle of milk",
                        Price = 2
                    },
                    new Product
                    {
                        Name = "Juice",
                        Description = "Orange Juice",
                        Price = 3
                    },
                    new Product
                    {
                        Name = "Water",
                        Description = "Bottle of water",
                        Price = 1
                    }
                );
                context.SaveChanges();
            }
            if (!context.RestaurantCategories.Any())
            {
                context.RestaurantCategories.AddRange(
                     new RestaurantCategory
                     {
                         Name = "Sushi and rolls" 
                     },
                     new RestaurantCategory
                     {
                        Name = "nice food"
                     }
                );
                context.SaveChanges();
            }
            
        }        
    }   
}
