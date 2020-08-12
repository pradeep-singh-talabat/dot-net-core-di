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

        public Worker(ILogger<Worker> logger, WorldWeatherService worldWeatherService)
        {
            this.logger = logger;
            this.worldWeatherService = worldWeatherService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await this.worldWeatherService.GetWeatherData().ConfigureAwait(false);
                this.logger.LogInformation($"Received Weather Data: [Temp: {result.Main.Temp}]");
                await Task.Delay(2000, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
