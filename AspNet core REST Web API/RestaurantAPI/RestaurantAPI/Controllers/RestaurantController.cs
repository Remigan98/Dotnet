using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Extensions;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
public class RestaurantController : ControllerBase
{
    RestaurantDbContext dbContext;
    IRestaurantService restaurantService;

    public RestaurantController(RestaurantDbContext dbContext, IRestaurantService restaurantService)
    {
        this.dbContext = dbContext;
        this.restaurantService = restaurantService;
    }

    [HttpPost("Create")]
    public ActionResult Create([FromBody] CreateRestaurantDto dto)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        int id = restaurantService.Create(dto);

        return Created($"/api/restaurant/{id}", null);
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        bool isUpdated = restaurantService.Update(id, dto);

        if (!isUpdated)
        {
            return NotFound();
        }

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
        RestaurantDto? restaurantDto = this.restaurantService.GetById(id);

        if (restaurantDto is null)
        {
            return NotFound();
        }

        return Ok(restaurantDto);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        bool isDeleted = restaurantService.Delete(id);

        return isDeleted ? NoContent() : NotFound();
    }
}

