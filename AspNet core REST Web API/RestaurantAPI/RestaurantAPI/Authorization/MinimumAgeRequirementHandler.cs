using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization;

public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    ILogger<MinimumAgeRequirementHandler> logger;

    public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
    {
        this.logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        DateTime dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth")?.Value ?? string.Empty);

        this.logger.LogInformation("User's date of birth: {dateOfBirth}", dateOfBirth);

        if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
        {
            this.logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            this.logger.LogInformation($"Authorization failed");
        }

        return Task.CompletedTask;
    }
}

