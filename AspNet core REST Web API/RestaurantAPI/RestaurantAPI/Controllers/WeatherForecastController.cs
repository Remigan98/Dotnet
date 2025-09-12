using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService service;
        private readonly ILogger<WeatherForecastController> logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var result = service.Get();

            return result;
        }

        [HttpGet("currentDay/{max}")]
        public IEnumerable<WeatherForecast> Get2([FromQuery] int take, [FromRoute] int max)
        {
            var result = service.Get();

            return result;
        }

        [HttpPost]
        public ActionResult<string> Hello([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return StatusCode(404);
            }

            return Ok($"Hello {name}");
        }
    }
}
