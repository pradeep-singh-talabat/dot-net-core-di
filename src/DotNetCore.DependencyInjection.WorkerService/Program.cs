namespace DotNetCore.DependencyInjection.WorkerService
{
    using System;
    using DotNetCore.DependencyInjection.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<WeatherConfiguration>(hostContext.Configuration.GetSection("weatherConfiguration"));
                    services.AddHttpClient<WorldWeatherService>(client => client.BaseAddress = new Uri(hostContext.Configuration.GetSection("weatherConfiguration:ApiUrl").Value));
                    services.AddHostedService<Worker>();
                });
    }
}
