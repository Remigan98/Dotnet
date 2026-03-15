using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.Repositories
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private GroceryDbContext _context = null!;
        private OrderRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            _context = TestDbContextFactory.Create();
            _repository = new OrderRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            TestDbContextFactory.Destroy(_context);
            _context?.Dispose();
        }

        [Test]
        public async Task GetByIdAsync_ShouldIncludeCustomerAndOrderItems()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "111"
            };

            var category = new Category { Name = "Test", Description = "Test" };
            var product = new Product { Name = "Product1", Price = 10.00m, Stock = 100, Category = category };

            var order = new Order
            {
                Customer = customer,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 20.00m,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Product = product, Quantity = 2, UnitPrice = 10.00m }
                }
            };

            _context.Customers.Add(customer);
            _context.Categories.Add(category);
            _context.Products.Add(product);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(order.Id, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Customer.Should().NotBeNull();
            result.Customer.FirstName.Should().Be("John");
            result.OrderItems.Should().HaveCount(1);
            result.OrderItems[0].Product.Should().NotBeNull();
            result.OrderItems[0].Product.Name.Should().Be("Product1");
        }

        [Test]
        public async Task GetByCustomerIdAsync_ShouldReturnCustomerOrders()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "111"
            };

            var order1 = new Order { Customer = customer, OrderDate = DateTime.UtcNow, TotalAmount = 10.00m, Status = OrderStatus.Pending };
            var order2 = new Order { Customer = customer, OrderDate = DateTime.UtcNow, TotalAmount = 20.00m, Status = OrderStatus.Confirmed };

            _context.Customers.Add(customer);
            _context.Orders.AddRange(order1, order2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByCustomerIdAsync(customer.Id, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
        }

        [Test]
        public async Task GetByStatusAsync_ShouldReturnOrdersWithStatus()
        {
            // Arrange
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john@test.com", PhoneNumber = "111" };

            var order1 = new Order { Customer = customer, OrderDate = DateTime.UtcNow, TotalAmount = 10.00m, Status = OrderStatus.Pending };
            var order2 = new Order { Customer = customer, OrderDate = DateTime.UtcNow, TotalAmount = 20.00m, Status = OrderStatus.Confirmed };
            var order3 = new Order { Customer = customer, OrderDate = DateTime.UtcNow, TotalAmount = 30.00m, Status = OrderStatus.Pending };

            _context.Customers.Add(customer);
            _context.Orders.AddRange(order1, order2, order3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByStatusAsync(OrderStatus.Pending, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.All(o => o.Status == OrderStatus.Pending).Should().BeTrue();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllOrders()
        {
            // Arrange
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john@test.com", PhoneNumber = "111" };

            var orders = new List<Order>
            {
                new Order { Customer = customer, OrderDate = DateTime.UtcNow, TotalAmount = 10.00m, Status = OrderStatus.Pending },
                new Order { Customer = customer, OrderDate = DateTime.UtcNow, TotalAmount = 20.00m, Status = OrderStatus.Confirmed },
                new Order { Customer = customer, OrderDate = DateTime.UtcNow, TotalAmount = 30.00m, Status = OrderStatus.Delivered }
            };

            _context.Customers.Add(customer);
            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().HaveCount(3);
        }

        [Test]
        public async Task AddAsync_ShouldAddOrder()
        {
            // Arrange
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john@test.com", PhoneNumber = "111" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var order = new Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 50.00m,
                Status = OrderStatus.Pending
            };

            // Act
            await _repository.AddAsync(order, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByIdAsync(order.Id, CancellationToken.None);
            result.Should().NotBeNull();
            result!.TotalAmount.Should().Be(50.00m);
        }
    }
}