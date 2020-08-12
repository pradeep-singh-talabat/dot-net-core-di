namespace DotNetCore.DependencyInjection.Api.Controllers
{
    using System.Threading.Tasks;
    using DotNetCore.DependencyInjection.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        private readonly SingletonWeatherService weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, SingletonWeatherService weatherService)
        {
            this.logger = logger;
            this.weatherService = weatherService;
        }

        [HttpGet]
        public async Task<WeatherResponse> Get() => await this.weatherService.GetWeatherData();
    }
}
