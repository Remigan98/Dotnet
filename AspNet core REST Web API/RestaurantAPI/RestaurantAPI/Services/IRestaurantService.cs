using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        void Update(int id, UpdateRestaurantDto dto);
        PageResult<RestaurantDto> GetAll(RestaurantQuery restaurantQuery);
        RestaurantDto GetById(int id);
        void Delete(int id);
    }
}