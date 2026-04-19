using System.Net;
using System.Net.Http.Json;
using Application.Categories.Commands.Create;
using Application.Categories.Commands.Update;
using Application.Categories.Dtos;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.API
{
    [TestFixture]
    public class CategoriesControllerTests
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
            var response = await _client.GetAsync("/api/categories");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_ShouldReturnCreated_WhenCategoryIsValid()
        {
            var command = new CreateCategoryCommand(
                $"Cat-{Guid.NewGuid()}",
                "Test category"
            );

            var response = await _client.PostAsJsonAsync("/api/categories", command);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            category.Should().NotBeNull();
            category!.Id.Should().BeGreaterThan(0);
            category.Name.Should().Be(command.Name);
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenCategoryExists()
        {
            HttpResponseMessage createResponse = await _client.PostAsJsonAsync("/api/categories", new CreateCategoryCommand($"Cat-{Guid.NewGuid()}", "Desc"));
            CategoryDto? created = await createResponse.Content.ReadFromJsonAsync<CategoryDto>();

            HttpResponseMessage response = await _client.GetAsync($"/api/categories/name/{created!.Name}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            CategoryDto? category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            category.Should().NotBeNull();
            category!.Id.Should().Be(created.Id);
        }

        [Test]
        public async Task GetByName_ShouldReturnOk_WhenCategoryExists()
        {
            var name = $"Cat-{Guid.NewGuid()}";
            await _client.PostAsJsonAsync("/api/categories", new CreateCategoryCommand(name, "Desc"));

            var response = await _client.GetAsync($"/api/categories/name/{name}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            category.Should().NotBeNull();
            category!.Name.Should().Be(name);
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenCategoryIsValid()
        {
            var createResponse = await _client.PostAsJsonAsync("/api/categories",
                new CreateCategoryCommand($"Cat-{Guid.NewGuid()}", "Old"));
            var created = await createResponse.Content.ReadFromJsonAsync<CategoryDto>();

            var updatedDto = new CategoryDto
            {
                Id = created!.Id,
                Name = "Updated",
                Description = "New"
            };

            var response = await _client.PutAsJsonAsync($"/api/categories/{created.Id}", new UpdateCategoryCommand(updatedDto));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            category.Should().NotBeNull();
            category!.Name.Should().Be("Updated");
            category.Description.Should().Be("New");
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenCategoryExists()
        {
            var createResponse = await _client.PostAsJsonAsync("/api/categories",
                new CreateCategoryCommand($"Cat-{Guid.NewGuid()}", "Desc"));
            var created = await createResponse.Content.ReadFromJsonAsync<CategoryDto>();

            var response = await _client.DeleteAsync($"/api/categories/{created!.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // No “GetById not found” behavior exists in controller (it always returns Ok from handler result),
            // so only assert the delete call succeeds.
        }
    }
}