using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Extensions;
using RestaurantAPI.Models;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    RestaurantDbContext dbContext;
    ILogger<RestaurantService> logger;

    public RestaurantService(RestaurantDbContext dbContext, ILogger<RestaurantService> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public int Create(CreateRestaurantDto dto)
    {
        Restaurant restaurant = dto.ToEntity();

        dbContext.Restaurants.Add(restaurant);
        dbContext.SaveChanges();

        return restaurant.Id;
    }

    public void Update(int id, UpdateRestaurantDto dto)
    {
        Restaurant? restaurant = dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null) throw new NotFoundException("Restaurant not found");

        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;

        dbContext.SaveChanges();
    }

    public RestaurantDto GetById(int id)
    {
        Restaurant? restaurant = dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null) throw new NotFoundException("Restaurant not found");

        return restaurant.ToDto();
    }

    public IEnumerable<RestaurantDto> GetAll()
    {
        List<Restaurant> restaurants = dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .ToList();

        return restaurants.ToDto();
    }

    public void Delete(int id)
    {
        logger.LogError($"Restaurant with id: {id} DELETE action invoked");

        Restaurant? restaurant = dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null) throw new NotFoundException("Restaurant not found");

        dbContext.Restaurants.Remove(restaurant);
        dbContext.SaveChanges();
    }
}
