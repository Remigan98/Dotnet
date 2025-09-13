using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Extensions;

public static class RestaurantMappingExtension
{
    public static RestaurantDto ToDto(this Restaurant restaurant)
    {
        RestaurantDto restaurantDto = new RestaurantDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Description = restaurant.Description,
            Category = restaurant.Category,
            HasDelivery = restaurant.HasDelivery,
            City = restaurant.Address.City,
            Street = restaurant.Address.Street,
            PostalCode = restaurant.Address.PostalCode,
            Dishes = restaurant.Dishes.Select(dish => new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price
            }).ToList()
        };

        return restaurantDto;
    }

    public static List<RestaurantDto> ToDto(this List<Restaurant> restaurants)
    {
        return restaurants.Select(r => r.ToDto()).ToList();
    }

    public static Restaurant ToEntity(this RestaurantDto restaurantDto)
    {
        return new Restaurant
        {
            Id = restaurantDto.Id,
            Name = restaurantDto.Name,
            Description = restaurantDto.Description,
            Category = restaurantDto.Category,
            HasDelivery = restaurantDto.HasDelivery,
            Address = new Address
            {
                City = restaurantDto.City,
                Street = restaurantDto.Street,
                PostalCode = restaurantDto.PostalCode
            },
            Dishes = restaurantDto.Dishes.Select(dishDto => new Dish
            {
                Id = dishDto.Id,
                Name = dishDto.Name,
                Description = dishDto.Description,
                Price = dishDto.Price
            }).ToList()
        };
    }

    public static Restaurant ToEntity(this CreateRestaurantDto createRestaurant)
    {
        return new Restaurant
        {
            Name = createRestaurant.Name,
            Description = createRestaurant.Description,
            Category = createRestaurant.Category,
            HasDelivery = createRestaurant.HasDelivery,
            ContactEmail = createRestaurant.ContactEmail,
            ContactNumber = createRestaurant.ContactNumber,
            Address = new Address
            {
                City = createRestaurant.City,
                Street = createRestaurant.Street,
                PostalCode = createRestaurant.PostalCode
            }
        };
    }
}