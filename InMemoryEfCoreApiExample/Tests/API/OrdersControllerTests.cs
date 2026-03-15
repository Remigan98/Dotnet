using System.Net;
using System.Net.Http.Json;
using Application.Customers.Commands.Create;
using Application.Customers.Dtos;
using Application.Orders.Commands.Create;
using Application.Orders.Dtos;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tests.API
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private CustomWebApplicationFactory _factory = null!;
        private HttpClient _client = null!;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/api/orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_ShouldReturnCreated_WhenOrderIsValid()
        {
            // Arrange - Create test data
            int customerId = await CreateTestCustomer();
            int productId = await CreateTestProduct();

            var command = new CreateOrderCommand(
                customerId,
                new List<CreateOrderItemCommand>
                {
                    new CreateOrderItemCommand(productId, 2)
                }
            );

            // Act
            var response = await _client.PostAsJsonAsync("/api/orders", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var order = await response.Content.ReadFromJsonAsync<OrderDto>();
            order.Should().NotBeNull();
            order!.CustomerId.Should().Be(customerId);
            order.OrderItems.Should().HaveCount(1);
            order.Status.Should().Be(OrderStatus.Pending);
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenOrderExists()
        {
            // Arrange
            int customerId = await CreateTestCustomer();
            int productId = await CreateTestProduct();

            var createCommand = new CreateOrderCommand(
                customerId,
                new List<CreateOrderItemCommand> { new CreateOrderItemCommand(productId, 1) }
            );

            var createResponse = await _client.PostAsJsonAsync("/api/orders", createCommand);
            var createdOrder = await createResponse.Content.ReadFromJsonAsync<OrderDto>();

            // Act
            var response = await _client.GetAsync($"/api/orders/{createdOrder!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var order = await response.Content.ReadFromJsonAsync<OrderDto>();
            order.Should().NotBeNull();
            order!.Id.Should().Be(createdOrder.Id);
        }

        [Test]
        public async Task Confirm_ShouldReturnOk_WhenOrderIsPending()
        {
            // Arrange
            int customerId = await CreateTestCustomer();
            int productId = await CreateTestProduct();

            var createCommand = new CreateOrderCommand(
                customerId,
                new List<CreateOrderItemCommand> { new CreateOrderItemCommand(productId, 1) }
            );

            var createResponse = await _client.PostAsJsonAsync("/api/orders", createCommand);
            var order = await createResponse.Content.ReadFromJsonAsync<OrderDto>();

            // Act
            var response = await _client.PostAsync($"/api/orders/{order!.Id}/confirm", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var confirmedOrder = await response.Content.ReadFromJsonAsync<OrderDto>();
            confirmedOrder!.Status.Should().Be(OrderStatus.Confirmed);
        }

        [Test]
        public async Task Cancel_ShouldReturnOk_WhenOrderIsNotDelivered()
        {
            // Arrange
            int customerId = await CreateTestCustomer();
            int productId = await CreateTestProduct();

            var createCommand = new CreateOrderCommand(
                customerId,
                new List<CreateOrderItemCommand> { new CreateOrderItemCommand(productId, 1) }
            );

            var createResponse = await _client.PostAsJsonAsync("/api/orders", createCommand);
            var order = await createResponse.Content.ReadFromJsonAsync<OrderDto>();

            // Act
            var response = await _client.PostAsync($"/api/orders/{order!.Id}/cancel", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var cancelledOrder = await response.Content.ReadFromJsonAsync<OrderDto>();
            cancelledOrder!.Status.Should().Be(OrderStatus.Cancelled);
        }

        [Test]
        public async Task GetByStatus_ShouldReturnOrdersWithSpecificStatus()
        {
            // Arrange
            int customerId = await CreateTestCustomer();
            int productId = await CreateTestProduct();

            // Create pending order
            await _client.PostAsJsonAsync("/api/orders", new CreateOrderCommand(
                customerId,
                new List<CreateOrderItemCommand> { new CreateOrderItemCommand(productId, 1) }
            ));

            // Act
            var response = await _client.GetAsync($"/api/orders/status/{OrderStatus.Pending}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
            orders.Should().NotBeEmpty();
            orders!.All(o => o.Status == OrderStatus.Pending).Should().BeTrue();
        }

        [Test]
        public async Task GetByCustomerId_ShouldReturnCustomerOrders()
        {
            // Arrange
            int customerId = await CreateTestCustomer();
            int productId = await CreateTestProduct();

            // Create two orders for the same customer
            await _client.PostAsJsonAsync("/api/orders", new CreateOrderCommand(
                customerId,
                new List<CreateOrderItemCommand> { new CreateOrderItemCommand(productId, 1) }
            ));

            await _client.PostAsJsonAsync("/api/orders", new CreateOrderCommand(
                customerId,
                new List<CreateOrderItemCommand> { new CreateOrderItemCommand(productId, 2) }
            ));

            // Act
            var response = await _client.GetAsync($"/api/orders/customer/{customerId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
            orders.Should().HaveCountGreaterThanOrEqualTo(2);
            orders!.All(o => o.CustomerId == customerId).Should().BeTrue();
        }

        private async Task<int> CreateTestCustomer()
        {
            var command = new CreateCustomerCommand(
                "Test",
                "Customer",
                $"test.{Guid.NewGuid()}@test.com",
                "1234567890"
            );

            var response = await _client.PostAsJsonAsync("/api/customers", command);
            var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
            return customer!.Id;
        }

        private async Task<int> CreateTestProduct()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GroceryDbContext>();

            var category = new Category { Name = $"TestCat-{Guid.NewGuid()}", Description = "Test" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var product = new Product
            {
                Name = $"TestProd-{Guid.NewGuid()}",
                Price = 10.00m,
                Stock = 100,
                CategoryId = category.Id
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return product.Id;
        }
    }
}