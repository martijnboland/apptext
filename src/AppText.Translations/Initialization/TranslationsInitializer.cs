using AppText.Features.ContentDefinition;
using AppText.Features.ContentDefinition.FieldTypes;
using AppText.Shared.Infrastructure;
using AppText.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppText.Translations.Initialization
{
    public class TranslationsInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public TranslationsInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Ensure that the global Translations content type exists
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TranslationsInitializer>>();
                logger.LogInformation($"Initializing AppText.Translations");

                var contentDefinitionStore = scope.ServiceProvider.GetRequiredService<IContentDefinitionStore>();
                var translationContentTypeQuery = new ContentTypeQuery { AppId = null, Name = "Translation" };
                var translationContentType = (await contentDefinitionStore.GetContentTypes(translationContentTypeQuery)).FirstOrDefault();
                if (translationContentType == null)
                {
                    logger.LogInformation($"Global Translation content type not found, creating...");
                    await contentDefinitionStore.AddContentType(new ContentType
                    {
                        AppId = null, // global
                        Name = Constants.TranslationContentType,
                        Description = "Built-in content type for translations",
                        ContentFields = new Field[]
                        {
                            new Field { Name = Constants.TranslationTextFieldName, Description = "Text", FieldType = new ShortText(), IsRequired = true }
                        },
                        Version = 0
                    });
                }
                else
                {
                    logger.LogInformation($"Global Translation content type already registered. Skipping creation...");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
