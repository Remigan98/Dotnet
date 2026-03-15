using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Commands.Create;
using Application.Orders.Dtos;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.Handlers
{
    [TestFixture]
    public class CreateOrderHandlerTests
    {
        private GroceryDbContext _context = null!;
        private IOrderRepository _orderRepository = null!;
        private ICustomerRepository _customerRepository = null!;
        private IProductRepository _productRepository = null!;
        private IUnitOfWork _unitOfWork = null!;
        private CreateOrderHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _context = TestDbContextFactory.Create();
            _orderRepository = new OrderRepository(_context);
            _customerRepository = new CustomerRepository(_context);
            _productRepository = new ProductRepository(_context);
            _unitOfWork = new UnitOfWork(_context);
            _handler = new CreateOrderHandler(_orderRepository, _customerRepository, _productRepository, _unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            TestDbContextFactory.Destroy(_context);
            _context?.Dispose();
        }

        [Test]
        public async Task Handle_ShouldCreateOrder_WhenCommandIsValid()
        {
            // Arrange
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john@test.com", PhoneNumber = "111" };
            var category = new Category { Name = "Test", Description = "Test" };
            var product = new Product { Name = "Product1", Price = 10.00m, Stock = 100, Category = category };

            _context.Customers.Add(customer);
            _context.Categories.Add(category);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var command = new CreateOrderCommand(
                customer.Id,
                new List<CreateOrderItemCommand>
                {
                    new CreateOrderItemCommand(product.Id, 2)
                }
            );

            // Act
            OrderDto result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.CustomerId.Should().Be(customer.Id);
            result.OrderItems.Should().HaveCount(1);
            result.TotalAmount.Should().Be(20.00m);
            result.Status.Should().Be(OrderStatus.Pending);

            // Verify stock was reduced
            var updatedProduct = await _productRepository.GetByIdAsync(product.Id, CancellationToken.None);
            updatedProduct!.Stock.Should().Be(98);
        }

        [Test]
        public void Handle_ShouldThrowNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var command = new CreateOrderCommand(
                999,
                new List<CreateOrderItemCommand>
                {
                    new CreateOrderItemCommand(1, 2)
                }
            );

            // Assert
            Assert.ThrowsAsync<NotFoundException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_ShouldThrowValidationException_WhenInsufficientStock()
        {
            // Arrange
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john@test.com", PhoneNumber = "111" };
            var category = new Category { Name = "Test", Description = "Test" };
            var product = new Product { Name = "Product1", Price = 10.00m, Stock = 5, Category = category };

            _context.Customers.Add(customer);
            _context.Categories.Add(category);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var command = new CreateOrderCommand(
                customer.Id,
                new List<CreateOrderItemCommand>
                {
                    new CreateOrderItemCommand(product.Id, 10) // More than available stock
                }
            );

            // Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }
    }
}