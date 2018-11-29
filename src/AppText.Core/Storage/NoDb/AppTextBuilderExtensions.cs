using AppText.Core.Application;
using AppText.Core.Configuration;
using AppText.Core.ContentDefinition;
using AppText.Core.ContentManagement;
using Microsoft.Extensions.DependencyInjection;
using NoDb;

namespace AppText.Core.Storage.NoDb
{
    public static class AppTextBuilderExtensions
    {
        public static void AddNoDbStorage(this AppTextBuilder builder, string baseFolder)
        {
            builder.Services.AddNoDb<App>();
            builder.Services.AddNoDb<ContentType>();
            builder.Services.AddNoDb<ContentCollection>();
            builder.Services.AddNoDb<ContentItem>();

            builder.Services.AddSingleton<IStoragePathOptionsResolver>(new AppTextStoragePathOptionsResolver(baseFolder));

            builder.Services.AddScoped<IApplicationStore, ApplicationStore>();
            builder.Services.AddScoped<IContentDefinitionStore, ContentDefinitionStore>();
            builder.Services.AddScoped<IContentStore, ContentStore>();
            builder.Services.AddScoped<IVersioner, Versioner>();
        }
    }
}
