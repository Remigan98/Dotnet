using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Extensions
{
    public static class DishMappingExtensions
    {
        public static Dish ToEntity(this CreateDishDto dto)
        {
            return new Dish
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                RestaurantId = dto.RestaurantId
            };
        }

        public static DishDto ToDto(this Dish dish)
        {
            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price
            };
        }

        public static List<DishDto> ToDtos(this List<Dish> dishes)
        {
            return dishes.Select(d => d.ToDto()).ToList();
        }
    }
}
