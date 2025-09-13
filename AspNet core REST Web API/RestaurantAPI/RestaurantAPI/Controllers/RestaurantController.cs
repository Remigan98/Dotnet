using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Extensions;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        RestaurantDbContext dbContext;

        public RestaurantController(RestaurantDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult Create([FromBody] CreateRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Restaurant restaurant = dto.ToEntity();

            dbContext.Restaurants.Add(restaurant);
            dbContext.SaveChanges();

            return Created($"/api/restaurant/{restaurant.Id}", null);
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            List<Restaurant> restaurants = dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes).ToList();

            List<RestaurantDto> restaurantDtos = restaurants.ToDto();

            return Ok(restaurantDtos);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            Restaurant? restaurant = dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                return NotFound();
            }

            RestaurantDto restaurantDto = restaurant.ToDto();

            return Ok(restaurantDto);
        }
    }
}
