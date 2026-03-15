using System.Net;
using System.Net.Http.Json;
using Application.Customers.Commands.Create;
using Application.Customers.Commands.Update;
using Application.Customers.Dtos;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.API
{
    [TestFixture]
    public class CustomersControllerTests
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
            var response = await _client.GetAsync("/api/customers");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_ShouldReturnCreated_WhenCustomerIsValid()
        {
            // Arrange
            var command = new CreateCustomerCommand(
                "John",
                "Doe",
                $"john.doe.{Guid.NewGuid()}@test.com",
                "1234567890"
            );

            // Act
            var response = await _client.PostAsJsonAsync("/api/customers", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
            customer.Should().NotBeNull();
            customer!.FirstName.Should().Be("John");
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenCustomerExists()
        {
            // Arrange - Create a customer first
            var createCommand = new CreateCustomerCommand(
                "Test",
                "User",
                $"test.{Guid.NewGuid()}@test.com",
                "1234567890"
            );

            var createResponse = await _client.PostAsJsonAsync("/api/customers", createCommand);
            var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerDto>();

            // Act
            var response = await _client.GetAsync($"/api/customers/{createdCustomer!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
            customer.Should().NotBeNull();
            customer!.Id.Should().Be(createdCustomer.Id);
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/customers/99999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenCustomerIsValid()
        {
            // Arrange - Create a customer first
            var email = $"update.{Guid.NewGuid()}@test.com";
            var createCommand = new CreateCustomerCommand("Original", "Name", email, "1234567890");
            var createResponse = await _client.PostAsJsonAsync("/api/customers", createCommand);
            var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerDto>();

            var updateCommand = new UpdateCustomerCommand(
                createdCustomer!.Id,
                "Updated",
                "Name",
                email,
                "0987654321"
            );

            // Act
            var response = await _client.PutAsJsonAsync($"/api/customers/{createdCustomer.Id}", updateCommand);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedCustomer = await response.Content.ReadFromJsonAsync<CustomerDto>();
            updatedCustomer!.FirstName.Should().Be("Updated");
            updatedCustomer.PhoneNumber.Should().Be("0987654321");
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenCustomerExists()
        {
            // Arrange - Create a customer first
            var createCommand = new CreateCustomerCommand(
                "Delete",
                "Me",
                $"delete.{Guid.NewGuid()}@test.com",
                "1234567890"
            );

            var createResponse = await _client.PostAsJsonAsync("/api/customers", createCommand);
            var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerDto>();

            // Act
            var response = await _client.DeleteAsync($"/api/customers/{createdCustomer!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify customer is deleted
            var getResponse = await _client.GetAsync($"/api/customers/{createdCustomer.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}