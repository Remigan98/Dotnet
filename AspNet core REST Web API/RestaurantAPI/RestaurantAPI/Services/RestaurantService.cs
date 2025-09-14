using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Extensions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    RestaurantDbContext dbContext;

    public RestaurantService(RestaurantDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public int Create(CreateRestaurantDto dto)
    {
        Restaurant restaurant = dto.ToEntity();

        dbContext.Restaurants.Add(restaurant);
        dbContext.SaveChanges();

        return restaurant.Id;
    }

    public bool Update(int id, UpdateRestaurantDto dto)
    {
        Restaurant? restaurant = dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null) return false;

        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;

        dbContext.SaveChanges();

        return true;
    }

    public RestaurantDto? GetById(int id)
    {
        Restaurant? restaurant = dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id);

        return restaurant?.ToDto();
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

    public bool Delete(int id)
    {
        Restaurant? restaurant = dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
        {
            return false;
        }

        dbContext.Restaurants.Remove(restaurant);
        dbContext.SaveChanges();

        return true;
    }
}
