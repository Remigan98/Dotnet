using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Restaurant retaurant)
        {
            if (requirement.ResourceOperation is ResourceOperationType.Create or ResourceOperationType.Read)
            {
                context.Succeed(requirement);
            }

            string? userID = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userID is null)
            {
                return Task.CompletedTask;
            }

            if (retaurant.CreatedById == int.Parse(userID))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
