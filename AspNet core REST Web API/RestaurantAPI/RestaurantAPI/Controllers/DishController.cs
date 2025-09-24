using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant/{restaurantId}/dish")]
[ApiController]
public class DishController : ControllerBase
{
    readonly IDishService dishService;

    public DishController(IDishService dishService)
    {
        this.dishService = dishService;
    }

    [HttpPost]
    public ActionResult Post([FromRoute] int restaurantID, [FromBody] CreateDishDto dto)
    {
        int newDishId = dishService.Create(restaurantID, dto);

        return Created($"/api/restaurant/{restaurantID}/dish/{newDishId}", null);
    }

    [HttpGet("{dishId}")]
    public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        DishDto dish = dishService.GetById(restaurantId, dishId);

        return Ok(dish);
    }

    [HttpGet]
    public ActionResult<List<DishDto>> Get([FromRoute] int restaurantId)
    {
        List<DishDto> dishes = dishService.GetAll(restaurantId);

        return Ok(dishes);
    }

    [HttpDelete]
    public ActionResult Delete([FromRoute] int restaurantId)
    {
        dishService.RemoveAll(restaurantId);

        return NoContent();
    }
}

