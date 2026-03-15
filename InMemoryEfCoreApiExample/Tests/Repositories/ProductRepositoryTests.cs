using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.Repositories
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private GroceryDbContext _context = null!;
        private ProductRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            _context = TestDbContextFactory.Create();
            _repository = new ProductRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            TestDbContextFactory.Destroy(_context);
            _context?.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            var category = new Category { Name = "Electronics", Description = "Electronic items" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var product = new Product
            {
                Name = "Laptop",
                Price = 999.99m,
                Stock = 10,
                CategoryId = category.Id
            };

            // Act
            await _repository.AddAsync(product, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByIdAsync(product.Id, CancellationToken.None);
            result.Should().NotBeNull();
            result!.Name.Should().Be("Laptop");
            result.Price.Should().Be(999.99m);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var category = new Category { Name = "Food", Description = "Food items" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var products = new List<Product>
            {
                new Product { Name = "Apple", Price = 1.50m, Stock = 100, CategoryId = category.Id },
                new Product { Name = "Banana", Price = 0.75m, Stock = 150, CategoryId = category.Id }
            };

            foreach (var product in products)
            {
                await _repository.AddAsync(product, CancellationToken.None);
            }
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            // Arrange
            var category = new Category { Name = "Food", Description = "Food items" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var product = new Product { Name = "Apple", Price = 1.50m, Stock = 100, CategoryId = category.Id };
            await _repository.AddAsync(product, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Act
            product.Price = 2.00m;
            product.Stock = 80;
            await _repository.UpdateAsync(product, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByIdAsync(product.Id, CancellationToken.None);
            result!.Price.Should().Be(2.00m);
            result.Stock.Should().Be(80);
        }
    }
}