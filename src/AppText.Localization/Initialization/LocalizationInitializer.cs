using AppText.Features.Application;
using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using AppText.Shared.Commands;
using AppText.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TranslationConstants = AppText.Translations.Constants;

namespace AppText.Localization.Initialization
{
    /// <summary>
    /// Initializer for AppText.Localization. It ensures that an app exists for Localization and that a collection exists for the resources.
    /// </summary>
    public class LocalizationInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppTextLocalizationOptions _options;
        private readonly ILogger<LocalizationInitializer> _logger;

        public LocalizationInitializer(IServiceProvider serviceProvider, IOptions<AppTextLocalizationOptions> options, ILogger<LocalizationInitializer> logger)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                _logger.LogInformation($"Initializing AppText.Localization...");

                var applicationStore = scopedServiceProvider.GetRequiredService<IApplicationStore>();
                var contentDefinitionStore = scopedServiceProvider.GetRequiredService<IContentDefinitionStore>();
                var contentStore = scopedServiceProvider.GetRequiredService<IContentStore>();

                var translationsContentType = (await contentDefinitionStore.GetContentTypes(new ContentTypeQuery
                {
                    AppId = null,
                    IncludeGlobalContentTypes = true,
                    Name = TranslationConstants.TranslationContentType,
                })).FirstOrDefault();
                if (translationsContentType == null)
                {
                    throw new InvalidOperationException($"Cannot initialize AppText.Localization because it requires the global {TranslationConstants.TranslationContentType} content type, which could not be found");
                }

                // Ensure there is an app where the translations are stored
                var appId = _options.AppId;
                if (! await applicationStore.AppExists(appId))
                {
                    await applicationStore.AddApp(new App
                    {
                        Id = appId,
                        DisplayName = appId,
                        Languages = new[] { Constants.DefaultDefaultLanguage },
                        DefaultLanguage = Constants.DefaultDefaultLanguage,
                        IsSystemApp = true
                    });
                }

                // Ensure there is a collection for the translations
                var collection = (await contentStore.GetContentCollections(new ContentCollectionQuery
                {
                    AppId = appId,
                    Name = _options.CollectionName
                })).FirstOrDefault();

                if (collection == null)
                {
                    collection = new ContentCollection
                    {
                        ContentType = translationsContentType,
                        Name = _options.CollectionName,
                        ListDisplayField = TranslationConstants.TranslationTextFieldName,
                        Version = 1
                    };
                    var contentCollectionSaveCommandHandler = scopedServiceProvider.GetRequiredService<ICommandHandler<SaveContentCollectionCommand>>();
                    await contentCollectionSaveCommandHandler.Handle(new SaveContentCollectionCommand(appId, collection));
                }
                _logger.LogInformation($"Finished initializing AppText.Localization.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
