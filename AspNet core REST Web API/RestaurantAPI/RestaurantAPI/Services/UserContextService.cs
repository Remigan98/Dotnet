using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public class UserContextService : IUserContextService
    {
        IHttpContextAccessor httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
        public int? UserId => int.TryParse(User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId) ? userId : null;
    }
}
