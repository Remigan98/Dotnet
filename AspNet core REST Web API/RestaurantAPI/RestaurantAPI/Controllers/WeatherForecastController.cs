using Microsoft.AspNetCore.Mvc;
using System.Collections;

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

        [HttpGet("currentDay/{minTemperature}/{maxTemperature}")]
        public IEnumerable<WeatherForecast> Get2([FromQuery] int take, [FromRoute] int minTemperature ,[FromRoute] int maxTemperature)
        {
            IEnumerable<WeatherForecast> result = service.Get();

            result = result.Where(x => x.TemperatureC >= minTemperature && x.TemperatureC <= maxTemperature).Take(take);

            return result;
        }

        public struct GenerateForecastRequest
        {
            public required int MinTemperature { get; set; }
            public required int MaxTemperature { get; set; }
        }

        [HttpPost("GenerateResults/{generatedResultAmount}")]
        public ActionResult<IEnumerable<WeatherForecast>> GenerateForecast([FromRoute] int generatedResultAmount, [FromBody] GenerateForecastRequest forecastRequest)
        {
            List<WeatherForecast> result = new List<WeatherForecast>();

            if (forecastRequest.MinTemperature > forecastRequest.MaxTemperature || generatedResultAmount <= 0)
            {
                return BadRequest();
            }

            Random random = new Random();

            for (int i = 0; i < generatedResultAmount; i++)
            {
                var temperatureC = random.Next(forecastRequest.MinTemperature, forecastRequest.MaxTemperature + 1);

                result.Add(new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                    TemperatureC = temperatureC,
                    Summary = "Generated"
                });
            }

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
