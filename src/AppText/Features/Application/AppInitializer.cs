using AppText.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class AppInitializer : IHostedService
    {
        private readonly string _appId;
        private readonly string _displayName;
        private readonly string[] _languages;
        private readonly string _defaultLanguage;
        private readonly IServiceProvider _serviceProvider;
        private readonly bool _isSystemApp;

        public AppInitializer(IServiceProvider serviceProvider, string appId, string displayName, string[] languages, string defaultLanguage, bool isSystemApp = false)
        {
            if (String.IsNullOrEmpty(appId))
            {
                throw new ArgumentException("The appId is requried for the AppInitializer");
            }

            _appId = appId;
            _displayName = displayName;
            _languages = languages;
            _defaultLanguage = defaultLanguage;
            _serviceProvider = serviceProvider;
            _isSystemApp = isSystemApp;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Ensure that the app exists.
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppInitializer>>();
                logger.LogInformation($"Initializing app {_appId}");

                var applicationStore = scope.ServiceProvider.GetRequiredService<IApplicationStore>();
                if (!await applicationStore.AppExists(_appId))
                {
                    logger.LogInformation($"App {_appId} doesn't exist yet. Initializing...");
                    await applicationStore.AddApp(new App
                    {
                        Id = _appId,
                        DisplayName = _displayName,
                        Languages = _languages,
                        DefaultLanguage = _defaultLanguage,
                        IsSystemApp = _isSystemApp
                    });
                    logger.LogInformation($"App {_appId} created and initialized");
                }
                else
                {
                    logger.LogInformation($"App {_appId} is already initialized");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
