namespace DotNetCore.DependencyInjection.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class WorldWeatherService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly string cityName;
        public WorldWeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.cityName = "dubai";
        }

        public async Task<WeatherResponse> GetWeatherData()
        {
            try
            {
                var apiKey = this.configuration.GetSection("ApiKey").Value;
                var result = await this.httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={this.cityName}&appid={apiKey}");
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(json);
                    return weatherResponse;
                }

                var response = result.Content.ReadAsStringAsync();
                throw new HttpRequestException($"failed to get data: [Status: {result.StatusCode} - {response}]");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
