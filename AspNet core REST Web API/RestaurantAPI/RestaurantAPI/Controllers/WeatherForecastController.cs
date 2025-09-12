using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class WeatherForecastController : ControllerBase
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
            var result = service.Get(6, -15, 50);

            return result;
        }

        [HttpGet("currentDay/{minTemperature}/{maxTemperature}")]
        public IEnumerable<WeatherForecast> Get2([FromQuery] int take, [FromRoute] int minTemperature ,[FromRoute] int maxTemperature)
        {
            IEnumerable<WeatherForecast> result = service.Get(take, minTemperature, maxTemperature);

            return result;
        }

        [HttpPost("GenerateResults/{count}")]
        public ActionResult<IEnumerable<WeatherForecast>> GenerateForecast([FromRoute] int count, [FromBody] GenerateForecastRequest request)
        {
            if (request.MinTemperature > request.MaxTemperature || count <= 0)
            {
                return BadRequest();
            }

            var result = service.Get(count, request.MinTemperature, request.MaxTemperature);

            return Ok(result);
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
