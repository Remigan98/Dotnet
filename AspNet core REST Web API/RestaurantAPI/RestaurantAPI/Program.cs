using RestaurantAPI;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddScoped<RestaurantSeeder>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    RestaurantSeeder seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
    seeder.Seed();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
