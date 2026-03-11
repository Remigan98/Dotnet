using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                GroceryDbContext context = scope.ServiceProvider.GetRequiredService<GroceryDbContext>();

                context.Database.EnsureCreated();

                if (context.Categories.Any() == false)
                {
                    List<Category> categories = new List<Category>()
                    {
                        new Category { Name = "Fruits", Description = "Fresh fruits" },
                        new Category { Name = "Vegetables", Description = "Fresh vegetables" },
                        new Category { Name = "Dairy", Description = "Milk, cheese, and more" },
                        new Category { Name = "Bakery", Description = "Bread, pastries, and more" },
                        new Category { Name = "Meat", Description = "Fresh meat and poultry" }
                    };

                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                }

                if (context.Products.Any() == false)
                {
                    int fruitsCategoryId = context.Categories.FirstOrDefault(c => c.Name == "Fruits")?.Id ?? 0;
                    List<Product> fruits = GetFruits(fruitsCategoryId);

                    int vegetablesCategoryId = context.Categories.FirstOrDefault(c => c.Name == "Vegetables")?.Id ?? 0;
                    List<Product> vegetables = GetVegetables(vegetablesCategoryId);

                    int dairyCategoryId = context.Categories.FirstOrDefault(c => c.Name == "Dairy")?.Id ?? 0;
                    List<Product> dairy = GetDairy(dairyCategoryId);

                    int bakeryCategoryId = context.Categories.FirstOrDefault(c => c.Name == "Bakery")?.Id ?? 0;
                    List<Product> bakery = GetBakery(bakeryCategoryId);

                    int meatCategoryId = context.Categories.FirstOrDefault(c => c.Name == "Meat")?.Id ?? 0;
                    List<Product> meat = GetMeat(meatCategoryId);

                    context.Products.AddRange(fruits);
                    context.Products.AddRange(vegetables);
                    context.Products.AddRange(dairy);
                    context.Products.AddRange(bakery);
                    context.Products.AddRange(meat);

                    context.SaveChanges();
                }
            }
        }

        static List<Product> GetFruits(int categoryId)
        {
            return new List<Product>()
            {
                new Product { Name = "Apple", Price = 0.5m, Stock = 100, CategoryId = categoryId },
                new Product { Name = "Banana", Price = 0.3m, Stock = 150, CategoryId = categoryId },
                new Product { Name = "Orange", Price = 0.4m, Stock = 120, CategoryId = categoryId },
                new Product { Name = "Grapes", Price = 1.0m, Stock = 80, CategoryId = categoryId },
                new Product { Name = "Strawberry", Price = 1.5m, Stock = 60, CategoryId = categoryId }
            };
        }

        static List<Product> GetVegetables(int categoryId)
        {
            return new List<Product>()
            {
                new Product { Name = "Carrot", Price = 0.2m, Stock = 200, CategoryId = categoryId },
                new Product { Name = "Broccoli", Price = 0.8m, Stock = 100, CategoryId = categoryId },
                new Product { Name = "Spinach", Price = 0.5m, Stock = 150, CategoryId = categoryId },
                new Product { Name = "Tomato", Price = 0.3m, Stock = 180, CategoryId = categoryId },
                new Product { Name = "Cucumber", Price = 0.4m, Stock = 120, CategoryId = categoryId }
            };
        }

        static List<Product> GetDairy(int categoryId)
        {
            return new List<Product>()
            {
                new Product { Name = "Milk", Price = 1.0m, Stock = 100, CategoryId = categoryId },
                new Product { Name = "Cheese", Price = 2.5m, Stock = 80, CategoryId = categoryId },
                new Product { Name = "Yogurt", Price = 0.8m, Stock = 150, CategoryId = categoryId },
                new Product { Name = "Butter", Price = 1.5m, Stock = 60, CategoryId = categoryId },
                new Product { Name = "Cream", Price = 1.2m, Stock = 90, CategoryId = categoryId }
            };
        }

        static List<Product> GetBakery(int categoryId)
        {
            return new List<Product>()
            {
                new Product { Name = "Bread", Price = 1.0m, Stock = 100, CategoryId = categoryId },
                new Product { Name = "Croissant", Price = 1.5m, Stock = 80, CategoryId = categoryId },
                new Product { Name = "Muffin", Price = 0.8m, Stock = 150, CategoryId = categoryId },
                new Product { Name = "Bagel", Price = 0.5m, Stock = 120, CategoryId = categoryId },
                new Product { Name = "Donut", Price = 0.7m, Stock = 90, CategoryId = categoryId }
            };
        }

        static List<Product> GetMeat(int categoryId)
        {
            return new List<Product>()
            {
                new Product { Name = "Chicken Breast", Price = 3.0m, Stock = 100, CategoryId = categoryId },
                new Product { Name = "Ground Beef", Price = 4.0m, Stock = 80, CategoryId = categoryId },
                new Product { Name = "Pork Chops", Price = 3.5m, Stock = 150, CategoryId = categoryId },
                new Product { Name = "Salmon", Price = 5.0m, Stock = 120, CategoryId = categoryId },
                new Product { Name = "Shrimp", Price = 6.0m, Stock = 90, CategoryId = categoryId }
            };
        }
    }
}
