namespace DotNetCore.DependencyInjection.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WeatherConfiguration
    {
        public string ApiUrl { get; set; }
        public string CityName { get; set; }
        public string ApiKey { get; set; }
    }
}
