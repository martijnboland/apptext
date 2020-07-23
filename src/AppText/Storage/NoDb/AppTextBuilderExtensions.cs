using AppText.Features.Application;
using AppText.Configuration;
using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using Microsoft.Extensions.DependencyInjection;
using NoDb;

namespace AppText.Storage.NoDb
{
    public static class AppTextBuilderExtensions
    {
        public static AppTextBuilder AddNoDbStorage(this AppTextBuilder builder, string baseFolder)
        {
            builder.Services.AddNoDb<App>();
            builder.Services.AddNoDb<ApiKey>();
            builder.Services.AddNoDb<ContentType>();
            builder.Services.AddNoDb<ContentCollection>();
            builder.Services.AddNoDb<ContentItem>();

            builder.Services.AddSingleton<IStoragePathOptionsResolver>(new AppTextStoragePathOptionsResolver(baseFolder));

            builder.Services.AddScoped<IApplicationStore, ApplicationStore>();
            builder.Services.AddScoped<IContentDefinitionStore, ContentDefinitionStore>();
            builder.Services.AddScoped<IContentStore, ContentStore>();
            builder.Services.AddScoped<IVersioner, Versioner>();

            return builder;
        }
    }
}
