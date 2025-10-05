using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public enum ResourceOperationType
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationType ResourceOperation { get; private set; }

        public ResourceOperationRequirement(ResourceOperationType operation)
        {
            ResourceOperation = operation;
        }
    }
}
