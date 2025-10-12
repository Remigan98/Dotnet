using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Extensions;
using RestaurantAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
[ApiController]
[Authorize]
public class RestaurantController : ControllerBase
{
    IRestaurantService restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        this.restaurantService = restaurantService;
    }

    [HttpPost("Create")]
    [Authorize(Roles = "Admin, Manager")]
    public ActionResult Create([FromBody] CreateRestaurantDto dto)
    {
        if (int.TryParse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            int id = restaurantService.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        return Unauthorized();
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
    {
        restaurantService.Update(id, dto);

        return Ok();
    }

    //[HttpGet("GetAll")]
    //[Authorize(Policy = "HasNationality")]
    //[Authorize(Policy = "AtLeast20")]
    //[Authorize(Policy = "CreatedAtLeast2Restaurants")]
    [HttpGet("GetAll")]
    [AllowAnonymous]
    public ActionResult<PageResult<RestaurantDto>> GetAll([FromQuery] RestaurantQuery restaurantQuery)
    {
        return Ok(restaurantService.GetAll(restaurantQuery));
    }

    [HttpGet]
    [Route("{id}")]
    [AllowAnonymous]
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

