using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto, int userId);
        void Update(int id, UpdateRestaurantDto dto, ClaimsPrincipal user);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);
        void Delete(int id, ClaimsPrincipal user);
    }
}