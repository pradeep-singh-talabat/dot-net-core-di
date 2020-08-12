namespace DotNetCore.DependencyInjection.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class SingletonWeatherService 
    {
        private readonly HttpClient httpClient;
        private readonly WeatherConfiguration configuration;
        private readonly ILogger<WorldWeatherService> logger;

        public SingletonWeatherService(IHttpClientFactory httpClientFactory, IOptionsMonitor<WeatherConfiguration> configuration, ILogger<WorldWeatherService> logger)
        {
            this.httpClient = httpClientFactory.CreateClient("weatherClient");
            this.configuration = configuration.CurrentValue;
            this.logger = logger;
        }

        public async Task<WeatherResponse> GetWeatherData()
        {
            try
            {
                var result = await this.httpClient.GetAsync($"data/2.5/weather?q={this.configuration.CityName}&appid={this.configuration.ApiKey}");
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(json);
                    return weatherResponse;
                }
                var response = result.Content.ReadAsStringAsync();
                this.logger.LogError($"HttpError: [Status: {result.StatusCode} - {response}]");
                throw new HttpRequestException($"failed to get data: [Status: {result.StatusCode} - {response}]");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Something went wrong, check the exception");
                throw;
            }
        }
    }
}
