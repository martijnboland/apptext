using AppText.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    /// <summary>
    /// Initializes pre-configured AppText apps at application startup.
    /// </summary>
    public class AppInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<App> _appsToInitialize;

        public AppInitializer(IServiceProvider serviceProvider, IOptions<AppInitializerOptions> options)
        {
            _serviceProvider = serviceProvider;
            _appsToInitialize = options.Value?.Apps ?? new List<App>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Ensure that the app exists.
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppInitializer>>();
                logger.LogInformation($"Initializing apps");

                var applicationStore = scope.ServiceProvider.GetRequiredService<IApplicationStore>();

                foreach (var app in _appsToInitialize)
                {
                    logger.LogInformation($"Initializing app {app.Id}");
                    if (!await applicationStore.AppExists(app.Id))
                    {
                        logger.LogInformation($"App {app.Id} doesn't exist yet. Initializing...");
                        await applicationStore.AddApp(app);
                        logger.LogInformation($"App {app.Id} created and initialized");
                    }
                    else
                    {
                        logger.LogInformation($"App {app.Id} is already initialized");
                    }

                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
