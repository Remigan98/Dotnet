using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                GroceryDbContext dbContext = scope.ServiceProvider.GetRequiredService<GroceryDbContext>();

                // Use EnsureCreatedAsync for in-memory databases
                await dbContext.Database.EnsureCreatedAsync();

                try
                {
                    if (dbContext.Categories.Any() == false)
                    {
                        await CreateCategoriesAsync(dbContext);
                    }

                    if (dbContext.Products.Any() == false)
                    {
                        await CreateProductsAsync(dbContext);
                    }

                    if (dbContext.Customers.Any() == false)
                    {
                        await CreateCustomersAsync(dbContext);
                    }

                    if (dbContext.Orders.Any() == false)
                    {
                        await CreateOrdersWithOrderItemsAsync(dbContext);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred seeding the database: {ex.Message}");
                    throw;
                }
            }
        }

        private static async Task CreateCategoriesAsync(GroceryDbContext context)
        {
            List<Category> categories = new List<Category>()
            {
                new Category { Name = "Fruits", Description = "Fresh fruits" },
                new Category { Name = "Vegetables", Description = "Fresh vegetables" },
                new Category { Name = "Dairy", Description = "Milk, cheese, and more" },
                new Category { Name = "Bakery", Description = "Bread, pastries, and more" },
                new Category { Name = "Meat", Description = "Fresh meat and poultry" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task CreateCustomersAsync(GroceryDbContext context)
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "123-456-7890" },
                new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "234-567-8901" },
                new Customer { FirstName = "Michael", LastName = "Johnson", Email = "michael.johnson@example.com", PhoneNumber = "345-678-9012" }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        private static async Task CreateOrdersWithOrderItemsAsync(GroceryDbContext context)
        {
            // Retrieve actual customers and products from DB
            List<Customer> customers = await context.Customers.ToListAsync();
            List<Product> products = await context.Products.ToListAsync();

            if (customers.Count == 0 || products.Count == 0)
            {
                Console.WriteLine("Cannot create orders: customers or products not found.");
                return;
            }

            // Create orders with varied dates
            DateTime baseDate = DateTime.Now;
            List<Order> orders = new List<Order>()
            {
                new Order 
                { 
                    CustomerId = customers[0].Id, 
                    OrderDate = baseDate.AddDays(-10),
                    Status = OrderStatus.Delivered
                },
                new Order 
                { 
                    CustomerId = customers[1].Id, 
                    OrderDate = baseDate.AddDays(-5),
                    Status = OrderStatus.Confirmed
                },
                new Order 
                { 
                    CustomerId = customers[2].Id, 
                    OrderDate = baseDate.AddDays(-2),
                    Status = OrderStatus.Pending
                },
                new Order 
                { 
                    CustomerId = customers[0].Id, 
                    OrderDate = baseDate.AddDays(-1),
                    Status = OrderStatus.Confirmed
                }
            };

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync(); // Save to generate Order IDs

            // Create OrderItems with actual product prices
            List<OrderItem> orderItems = new List<OrderItem>()
            {
                // Order 1 - John's first order (Apples and Bananas)
                new OrderItem 
                { 
                    OrderId = orders[0].Id, 
                    ProductId = products.First(p => p.Name == "Apple").Id, 
                    Quantity = 5, 
                    UnitPrice = products.First(p => p.Name == "Apple").Price 
                },
                new OrderItem 
                { 
                    OrderId = orders[0].Id, 
                    ProductId = products.First(p => p.Name == "Banana").Id, 
                    Quantity = 10, 
                    UnitPrice = products.First(p => p.Name == "Banana").Price 
                },
                new OrderItem 
                { 
                    OrderId = orders[0].Id, 
                    ProductId = products.First(p => p.Name == "Milk").Id, 
                    Quantity = 2, 
                    UnitPrice = products.First(p => p.Name == "Milk").Price 
                },

                // Order 2 - Jane's order (Vegetables and Dairy)
                new OrderItem 
                { 
                    OrderId = orders[1].Id, 
                    ProductId = products.First(p => p.Name == "Carrot").Id, 
                    Quantity = 3, 
                    UnitPrice = products.First(p => p.Name == "Carrot").Price 
                },
                new OrderItem 
                { 
                    OrderId = orders[1].Id, 
                    ProductId = products.First(p => p.Name == "Broccoli").Id, 
                    Quantity = 2, 
                    UnitPrice = products.First(p => p.Name == "Broccoli").Price 
                },
                new OrderItem 
                { 
                    OrderId = orders[1].Id, 
                    ProductId = products.First(p => p.Name == "Cheese").Id, 
                    Quantity = 1, 
                    UnitPrice = products.First(p => p.Name == "Cheese").Price 
                },

                // Order 3 - Michael's order (Bakery and Meat)
                new OrderItem 
                { 
                    OrderId = orders[2].Id, 
                    ProductId = products.First(p => p.Name == "Bread").Id, 
                    Quantity = 2, 
                    UnitPrice = products.First(p => p.Name == "Bread").Price 
                },
                new OrderItem 
                { 
                    OrderId = orders[2].Id, 
                    ProductId = products.First(p => p.Name == "Chicken Breast").Id, 
                    Quantity = 1, 
                    UnitPrice = products.First(p => p.Name == "Chicken Breast").Price 
                },

                // Order 4 - John's second order (Seafood and Fruit)
                new OrderItem 
                { 
                    OrderId = orders[3].Id, 
                    ProductId = products.First(p => p.Name == "Salmon").Id, 
                    Quantity = 2, 
                    UnitPrice = products.First(p => p.Name == "Salmon").Price 
                },
                new OrderItem 
                { 
                    OrderId = orders[3].Id, 
                    ProductId = products.First(p => p.Name == "Strawberry").Id, 
                    Quantity = 3, 
                    UnitPrice = products.First(p => p.Name == "Strawberry").Price 
                }
            };

            await context.OrderItems.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();

            // Calculate and update TotalAmount for each order based on actual order items
            foreach (Order order in orders)
            {
                order.TotalAmount = await context.OrderItems
                    .Where(oi => oi.OrderId == order.Id)
                    .SumAsync(oi => oi.Quantity * oi.UnitPrice);
            }

            await context.SaveChangesAsync(); // Save updated totals
        }

        #region Seed Products
        private static async Task CreateProductsAsync(GroceryDbContext dbContext)
        {
            int fruitsCategoryId = (await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Fruits"))?.Id ?? 0;
            List<Product> fruits = GetFruits(fruitsCategoryId);

            int vegetablesCategoryId = (await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Vegetables"))?.Id ?? 0;
            List<Product> vegetables = GetVegetables(vegetablesCategoryId);

            int dairyCategoryId = (await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Dairy"))?.Id ?? 0;
            List<Product> dairy = GetDairy(dairyCategoryId);

            int bakeryCategoryId = (await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Bakery"))?.Id ?? 0;
            List<Product> bakery = GetBakery(bakeryCategoryId);

            int meatCategoryId = (await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Meat"))?.Id ?? 0;
            List<Product> meat = GetMeat(meatCategoryId);

            await dbContext.Products.AddRangeAsync(fruits);
            await dbContext.Products.AddRangeAsync(vegetables);
            await dbContext.Products.AddRangeAsync(dairy);
            await dbContext.Products.AddRangeAsync(bakery);
            await dbContext.Products.AddRangeAsync(meat);

            await dbContext.SaveChangesAsync();
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
        #endregion
    }
}
