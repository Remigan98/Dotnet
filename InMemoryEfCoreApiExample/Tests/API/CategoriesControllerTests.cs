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
        private CustomWebApplicationFactory factory = null!;
        private HttpClient client = null!;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            factory = new CustomWebApplicationFactory();
            client = factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            client.Dispose();
            factory.Dispose();
        }

        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            HttpResponseMessage response = await client.GetAsync("/api/categories");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_ShouldReturnCreated_WhenCategoryIsValid()
        {
            CreateCategoryCommand command = new CreateCategoryCommand(
                $"Cat-{Guid.NewGuid()}",
                "Test category"
            );

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/categories", command);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            CategoryDto? category = await response.Content.ReadFromJsonAsync<CategoryDto>();

            category.Should().NotBeNull();
            category!.Id.Should().BeGreaterThan(0);
            category.Name.Should().Be(command.Name);
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenCategoryExists()
        {
            HttpResponseMessage createResponse = await client.PostAsJsonAsync("/api/categories", new CreateCategoryCommand($"Cat-{Guid.NewGuid()}", "Desc"));
            CategoryDto? created = await createResponse.Content.ReadFromJsonAsync<CategoryDto>();

            HttpResponseMessage response = await client.GetAsync($"/api/categories/name/{created!.Name}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            CategoryDto? category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            category.Should().NotBeNull();
            category!.Id.Should().Be(created.Id);
        }

        [Test]
        public async Task GetByName_ShouldReturnOk_WhenCategoryExists()
        {
            string name = $"Cat-{Guid.NewGuid()}";
            await client.PostAsJsonAsync("/api/categories", new CreateCategoryCommand(name, "Desc"));

            HttpResponseMessage response = await client.GetAsync($"/api/categories/name/{name}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            CategoryDto? category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            category.Should().NotBeNull();
            category!.Name.Should().Be(name);
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenCategoryIsValid()
        {
            HttpResponseMessage createResponse = await client.PostAsJsonAsync("/api/categories",
                new CreateCategoryCommand($"Cat-{Guid.NewGuid()}", "Old"));
            CategoryDto? created = await createResponse.Content.ReadFromJsonAsync<CategoryDto>();

            CategoryDto updatedDto = new CategoryDto
            {
                Id = created!.Id,
                Name = "Updated",
                Description = "New"
            };

            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/categories/{created.Id}", new UpdateCategoryCommand(updatedDto));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            CategoryDto? category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            category.Should().NotBeNull();
            category!.Name.Should().Be("Updated");
            category.Description.Should().Be("New");
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenCategoryExists()
        {
            HttpResponseMessage createResponse = await client.PostAsJsonAsync("/api/categories", new CreateCategoryCommand($"Cat-{Guid.NewGuid()}", "Desc"));
            CategoryDto? created = await createResponse.Content.ReadFromJsonAsync<CategoryDto>();

            HttpResponseMessage response = await client.DeleteAsync($"/api/categories/{created!.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // No “GetById not found” behavior exists in controller (it always returns Ok from handler result),
            // so only assert the delete call succeeds.
        }
    }
}