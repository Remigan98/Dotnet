using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Extensions;
using RestaurantAPI.Models;
using RestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Authorization;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    RestaurantDbContext dbContext;
    ILogger<RestaurantService> logger;
    IAuthorizationService authorizationService;

    public RestaurantService(RestaurantDbContext dbContext, ILogger<RestaurantService> logger, IAuthorizationService authorizationService)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.authorizationService = authorizationService;
    }

    public int Create(CreateRestaurantDto dto, int userId)
    {
        Restaurant restaurant = dto.ToEntity();
        restaurant.CreatedById = userId;

        dbContext.Restaurants.Add(restaurant);
        dbContext.SaveChanges();

        return restaurant.Id;
    }

    public void Update(int id, UpdateRestaurantDto dto, ClaimsPrincipal user)
    {
        Restaurant restaurant = dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id) ?? throw new NotFoundException("Restaurant not found");

        AuthorizationResult authorizationResult = authorizationService
            .AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperationType.Update))
            .Result;

        if (authorizationResult.Succeeded == false)
        {
            throw new ForbidException();
        }
        
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

        return restaurant is null ? throw new NotFoundException("Restaurant not found") : restaurant.ToDto();
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

    public void Delete(int id, ClaimsPrincipal user)
    {
        logger.LogError($"Restaurant with id: {id} DELETE action invoked");

        Restaurant restaurant = dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id) ?? throw new NotFoundException("Restaurant not found");

        AuthorizationResult authorizationResult = authorizationService
            .AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperationType.Delete))
            .Result;

        if (authorizationResult.Succeeded == false)
        {
            throw new ForbidException();
        }

        dbContext.Restaurants.Remove(restaurant);
        dbContext.SaveChanges();
    }
}
