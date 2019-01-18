using AppText.Shared.Infrastructure;
using AppText.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AppText.Features.Application
{
    public class AppInitializer : IStartupFilter
    {
        private readonly string _appId;
        private readonly string _displayName;
        private readonly string[] _languages;
        private readonly string _defaultLanguage;
        private readonly IServiceProvider _serviceProvider;

        public AppInitializer(IServiceProvider serviceProvider, string appId, string displayName, string[] languages, string defaultLanguage)
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
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            // Ensure that the app exists.
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppInitializer>>();
                logger.LogInformation($"Initializing app {_appId}");

                var applicationStore = scope.ServiceProvider.GetRequiredService<IApplicationStore>();
                if (!AsyncHelper.RunSync(() => applicationStore.AppExists(_appId)))
                {
                    logger.LogInformation($"App {_appId} doesn't exist yet. Initializing...");
                    AsyncHelper.RunSync(() => applicationStore.AddApp(new App
                    {
                        Id = _appId,
                        DisplayName = _displayName,
                        Languages = _languages,
                        DefaultLanguage = _defaultLanguage
                    }));
                    logger.LogInformation($"App {_appId} created and initialized");
                }
                else
                {
                    logger.LogInformation($"App {_appId} is already initialized");
                }
            }
            return next;
        }
    }
}
