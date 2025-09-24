using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Extensions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext context;

        public DishService(RestaurantDbContext context)
        {
            this.context = context;
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            Restaurant restaurant = context.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId)
                ?? throw new NotFoundException("Restaurant not found");

            return restaurant;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            Restaurant restaurant = GetRestaurantById(restaurantId);
            Dish dishEntity = dto.ToEntity();

            dishEntity.RestaurantId = restaurantId;

            context.Dishes.Add(dishEntity);
            context.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            Restaurant restaurant = GetRestaurantById(restaurantId);
            Dish dish = context.Dishes
                .FirstOrDefault(d => d.Id == dishId && d.RestaurantId == restaurantId)
                ?? throw new NotFoundException("Dish not found");

            return dish.ToDto();
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            Restaurant restaurant = GetRestaurantById(restaurantId);
            List<DishDto> dishDtos = context.Dishes.ToList().ToDtos();

            return dishDtos;
        }

        public void RemoveAll(int restaurantId)
        {
            Restaurant restaurant = GetRestaurantById(restaurantId);

            context.Dishes.RemoveRange(restaurant.Dishes);
            context.SaveChanges();
        }
    }
}
