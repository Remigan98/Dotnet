using System.Net;
using System.Net.Http.Json;
using Application.Categories.Commands.Create;
using Application.Categories.Dtos;
using Application.Products.Commands.Create;
using Application.Products.Commands.Update;
using Application.Products.Dtos;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.API
{
    [TestFixture]
    public class ProductsControllerTests
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
            var response = await _client.GetAsync("/api/products");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_ShouldReturnCreated_WhenProductIsValid()
        {
            // Arrange
            var categoryId = await CreateTestCategoryId();

            var command = new CreateProductCommand(
                $"Prod-{Guid.NewGuid()}",
                12.34m,
                10,
                categoryId
            );

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            product.Should().NotBeNull();
            product!.Id.Should().BeGreaterThan(0);
            product.Name.Should().Be(command.Name);
            product.CategoryId.Should().Be(categoryId);
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var categoryId = await CreateTestCategoryId();
            var createResponse = await _client.PostAsJsonAsync("/api/products",
                new CreateProductCommand($"Prod-{Guid.NewGuid()}", 1.23m, 5, categoryId));
            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

            // Act
            var response = await _client.GetAsync($"/api/products/{created!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            product.Should().NotBeNull();
            product!.Id.Should().Be(created.Id);
        }

        [Test]
        public async Task GetByCategoryId_ShouldReturnOk()
        {
            // Arrange
            var categoryId = await CreateTestCategoryId();
            await _client.PostAsJsonAsync("/api/products",
                new CreateProductCommand($"Prod-{Guid.NewGuid()}", 1.00m, 1, categoryId));

            // Act
            var response = await _client.GetAsync($"/api/products/category/{categoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            products.Should().NotBeNull();
            products!.All(p => p.CategoryId == categoryId).Should().BeTrue();
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenProductIsValid()
        {
            // Arrange
            var categoryId = await CreateTestCategoryId();
            var createResponse = await _client.PostAsJsonAsync("/api/products",
                new CreateProductCommand($"Prod-{Guid.NewGuid()}", 5.00m, 10, categoryId));
            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

            var updateCommand = new UpdateProductCommand(
                created!.Id,
                "Updated",
                9.99m,
                categoryId
            );

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{created.Id}", updateCommand);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await response.Content.ReadFromJsonAsync<ProductDto>();
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Updated");
            updated.Price.Should().Be(9.99m);
        }

        [Test]
        public async Task Update_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var categoryId = await CreateTestCategoryId();
            var createResponse = await _client.PostAsJsonAsync("/api/products",
                new CreateProductCommand($"Prod-{Guid.NewGuid()}", 5.00m, 10, categoryId));
            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

            var updateCommand = new UpdateProductCommand(
                created!.Id + 1,
                "Updated",
                9.99m,
                categoryId
            );

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{created.Id}", updateCommand);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var categoryId = await CreateTestCategoryId();
            var createResponse = await _client.PostAsJsonAsync("/api/products",
                new CreateProductCommand($"Prod-{Guid.NewGuid()}", 5.00m, 10, categoryId));
            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

            // Act
            var response = await _client.DeleteAsync($"/api/products/{created!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task<int> CreateTestCategoryId()
        {
            var response = await _client.PostAsJsonAsync("/api/categories",
                new CreateCategoryCommand($"Cat-{Guid.NewGuid()}", "Desc"));

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            category.Should().NotBeNull();
            return category!.Id;
        }
    }
}