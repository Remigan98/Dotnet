using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization;

public class MinimumRestaurantsCreatedHandler : AuthorizationHandler<MinimumRestaurantsCreatedRequirement>
{
    readonly RestaurantDbContext dbContext;

    public MinimumRestaurantsCreatedHandler(RestaurantDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantsCreatedRequirement requirement)
    {
        int userID = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

        int restaurantsCreated = 
            dbContext
            .Restaurants
            .Count(r => r.CreatedById == userID);

        if (restaurantsCreated >= requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
