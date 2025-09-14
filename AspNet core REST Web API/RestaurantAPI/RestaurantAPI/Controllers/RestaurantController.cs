using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Extensions;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
[ApiController]
public class RestaurantController : ControllerBase
{
    IRestaurantService restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        this.restaurantService = restaurantService;
    }

    [HttpPost("Create")]
    public ActionResult Create([FromBody] CreateRestaurantDto dto)
    {
        int id = restaurantService.Create(dto);

        return Created($"/api/restaurant/{id}", null);
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
    {
        restaurantService.Update(id, dto);

        return Ok();
    }

    [HttpGet("GetAll")]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        return Ok(restaurantService.GetAll());
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<RestaurantDto> Get([FromRoute] int id)
    {
        RestaurantDto restaurantDto = this.restaurantService.GetById(id);

        return Ok(restaurantDto);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        restaurantService.Delete(id);

        return NoContent();
    }
}

