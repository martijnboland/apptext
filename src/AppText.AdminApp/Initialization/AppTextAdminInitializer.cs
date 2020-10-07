using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using AppText.Shared.Commands;
using AppText.Shared.Infrastructure;
using AppText.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TranslationConstants = AppText.Translations.Constants;

namespace AppText.AdminApp.Initialization
{
    public class AppTextAdminInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private IDirectoryContents _translationsContents;
        private readonly ILogger<AppTextAdminInitializer> _logger;

        public AppTextAdminInitializer(ILogger<AppTextAdminInitializer> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var embeddedProvider = new EmbeddedFileProvider(typeof(AppTextAdminInitializer).Assembly);
            _translationsContents = embeddedProvider.GetDirectoryContents("");
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                _logger.LogInformation($"Initializing AppText.AdminApp...");
                
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
                    throw new InvalidOperationException($"Cannot initialize AppText.AdminApp because it requires the global {TranslationConstants.TranslationContentType} content type, which could not be found");
                }

                // Ensure collections
                foreach(var collectionName in Constants.AppTextAdminCollections)
                {
                    _logger.LogInformation("Initializing content collection {0}", collectionName);
                    await InitCollection(collectionName, contentStore, translationsContentType, scopedServiceProvider);
                }
                _logger.LogInformation($"Finished initializing AppText.AdminApp.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task InitCollection(string collectionName, IContentStore contentStore, ContentType translationsContentType, IServiceProvider serviceProvider)
        {
            var collection = (await contentStore.GetContentCollections(new ContentCollectionQuery
            {
                AppId = Constants.AppTextAdminAppId,
                Name = collectionName
            })).FirstOrDefault();

            if (collection == null)
            {
                collection = new ContentCollection
                {
                    ContentType = translationsContentType,
                    Name = collectionName,
                    ListDisplayField = TranslationConstants.TranslationTextFieldName,
                    Version = 1
                };
                var contentCollectionSaveCommandHandler = serviceProvider.GetRequiredService<ICommandHandler<SaveContentCollectionCommand>>();
                await contentCollectionSaveCommandHandler.Handle(new SaveContentCollectionCommand(Constants.AppTextAdminAppId, collection));
            }

            // Read content from embedded json files and add to storage
            var contentFiles = _translationsContents.Where(tc => tc.Name.StartsWith($"Content.{collectionName}"));
            var initPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "apptextadmin-init") }));
            var saveContentItemCommandHandler = new SaveContentItemCommandHandler(
                serviceProvider.GetRequiredService<IContentStore>(),
                serviceProvider.GetRequiredService<IVersioner>(),
                serviceProvider.GetRequiredService<ContentItemValidator>(),
                initPrincipal,
                serviceProvider.GetRequiredService<IDispatcher>());
            foreach (var contentFile in contentFiles)
            {
                _logger.LogInformation("Syncing content from {0}", contentFile.Name);

                // Read/refresh exisiting content items before reading content file
                var existingContentItems = await contentStore.GetContentItems(new ContentItemQuery
                {
                    AppId = Constants.AppTextAdminAppId,
                    CollectionId = collection.Id
                });

                var language = contentFile.Name.Substring($"Content.{collectionName}".Length + 1).Replace(".json", String.Empty);
                using (var contentStream = contentFile.CreateReadStream())
                using (var sr = new StreamReader(contentStream))
                using (var jsonReader = new JsonTextReader(sr))
                {
                    var items = JArray.Load(jsonReader).Children<JObject>();
                    foreach (var item in items)
                    {
                        foreach (var prop in item.Properties())
                        {
                            var saveContentItemCommand = new SaveContentItemCommand
                            {
                                AppId = Constants.AppTextAdminAppId,
                                CollectionId = collection.Id,
                                LanguagesToValidate = new[] { language },
                                ContentKey = prop.Name
                            };
                            var contentItem = existingContentItems.FirstOrDefault(ci => ci.ContentKey == prop.Name);
                            // Only store text when contentItem does not exist, or when the text is changed and the contentItem was not edited by
                            // another user than apptextadmin-init
                            var contentFieldValue = contentItem != null
                                ? JObject.FromObject(contentItem.Content[TranslationConstants.TranslationTextFieldName])
                                : new JObject();
                            if (contentItem == null ||
                                (contentFieldValue[language]?.ToString() != prop.Value?.ToString() && contentItem.LastModifiedBy == "apptextadmin-init"))
                            {
                                if (contentItem != null)
                                {
                                    _logger.LogDebug("Updating content item with key {0}", prop.Name);
                                    saveContentItemCommand.Id = contentItem.Id;
                                    saveContentItemCommand.Version = contentItem.Version;
                                    saveContentItemCommand.Content = contentItem.Content;
                                }
                                else
                                {
                                    _logger.LogDebug("Creating new content item with key {0}", prop.Name);
                                }

                                contentFieldValue[language] = prop.Value;
                                saveContentItemCommand.Content[TranslationConstants.TranslationTextFieldName] = contentFieldValue;

                                var result = await saveContentItemCommandHandler.Handle(saveContentItemCommand);
                                if (result.Status != ResultStatus.Success)
                                {
                                    _logger.LogWarning("Error initializing content item with key {0} and language {1}: {2}",
                                        saveContentItemCommand.ContentKey, language, String.Join(";", result.ValidationErrors.Select(err => err.ErrorMessage)));
                                }
                            }
                            else
                            {
                                _logger.LogDebug("Content for content items with key {0} is not changed, or the content in the database has been changed by the user and we won't overwrite that. Skipping update...", prop.Name);
                            }
                        }
                    }
                }
            }
        }
    }
}
