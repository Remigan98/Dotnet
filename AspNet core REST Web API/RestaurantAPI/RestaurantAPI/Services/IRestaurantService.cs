using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        bool Update(int id, UpdateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto? GetById(int id);
        bool Delete(int id);
    }
}