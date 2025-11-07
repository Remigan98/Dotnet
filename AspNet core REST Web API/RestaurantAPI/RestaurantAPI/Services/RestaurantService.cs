using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Extensions;
using RestaurantAPI.Models;
using RestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    RestaurantDbContext dbContext;
    ILogger<RestaurantService> logger;
    IAuthorizationService authorizationService;
    IUserContextService userContextService;

    public RestaurantService(RestaurantDbContext dbContext, ILogger<RestaurantService> logger, 
        IAuthorizationService authorizationService,
        IUserContextService userContextService)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.authorizationService = authorizationService;
        this.userContextService = userContextService;
    }

    public int Create(CreateRestaurantDto dto)
    {
        Restaurant restaurant = dto.ToEntity();
        restaurant.CreatedById = userContextService.UserId;

        dbContext.Restaurants.Add(restaurant);
        dbContext.SaveChanges();

        return restaurant.Id;
    }

    public void Update(int id, UpdateRestaurantDto dto)
    {
        if (userContextService.User is null)
        {
            throw new ForbidException();
        }

        Restaurant restaurant = dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id) ?? throw new NotFoundException("Restaurant not found");

        AuthorizationResult authorizationResult = authorizationService
            .AuthorizeAsync(userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperationType.Update))
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

    public PageResult<RestaurantDto> GetAll(RestaurantQuery query)
    {
        IQueryable<Restaurant> baseQuery = dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .Where(r => query.SearchPhrase == null ||
                    (r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Description.ToLower().Contains(query.SearchPhrase)));

        if (string.IsNullOrEmpty(query.SortBy) == false)
        {
            var columnsSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Category), r => r.Category },
                { nameof(Restaurant.Description), r => r.Description }
            };

            Expression<Func<Restaurant, object>> selectedColumn = columnsSelectors[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        List<Restaurant> restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        PageResult<RestaurantDto> result = new PageResult<RestaurantDto>(restaurants.ToDto(), restaurants.Count(), query.PageSize, query.PageNumber);

        return result;
    }

    public void Delete(int id)
    {
        if (userContextService.User is null)
        {
            throw new ForbidException();
        }

        logger.LogError($"Restaurant with id: {id} DELETE action invoked");

        Restaurant restaurant = dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id) ?? throw new NotFoundException("Restaurant not found");

        AuthorizationResult authorizationResult = authorizationService
            .AuthorizeAsync(userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperationType.Delete))
            .Result;

        if (authorizationResult.Succeeded == false)
        {
            throw new ForbidException();
        }

        dbContext.Restaurants.Remove(restaurant);
        dbContext.SaveChanges();
    }
}
