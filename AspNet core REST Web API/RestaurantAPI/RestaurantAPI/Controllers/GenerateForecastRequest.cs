namespace RestaurantAPI.Controllers
{
    public partial class WeatherForecastController
    {
        public struct GenerateForecastRequest
        {
            public required int MinTemperature { get; set; }
            public required int MaxTemperature { get; set; }
        }
    }
}
