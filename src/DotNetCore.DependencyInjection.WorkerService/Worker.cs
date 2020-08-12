namespace DotNetCore.DependencyInjection.WorkerService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetCore.DependencyInjection.Services;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly WorldWeatherService worldWeatherService;
        private readonly SingletonWeatherService worldWeatherServiceSingleton;

        public Worker(ILogger<Worker> logger, WorldWeatherService worldWeatherService, SingletonWeatherService worldWeatherServiceSingleton)
        {
            this.logger = logger;
            this.worldWeatherService = worldWeatherService;
            this.worldWeatherServiceSingleton = worldWeatherServiceSingleton;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if(DateTime.Now.Second%2==0)
                {
                    var result = await this.worldWeatherService.GetWeatherData().ConfigureAwait(false);
                    this.logger.LogInformation($"##############################\n\tTransientClient: Received Weather Data: [Temp: {result.Main.Temp}]\n\t##############################");
                }
                else
                {
                    var result = await this.worldWeatherServiceSingleton.GetWeatherData().ConfigureAwait(false);
                    this.logger.LogInformation($"******************************\n\tSingletonClient: Received Weather Data: [Temp: {result.Main.Temp}]\n\t******************************");
                }
                await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
