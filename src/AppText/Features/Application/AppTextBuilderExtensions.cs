using AppText.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Features.Application
{
    public static class AppTextBuilderExtensions
    {
        public static AppTextBuilder InitializeApp(this AppTextBuilder builder, string appId, string displayName, string[] languages, string defaultLanguage, bool isSystem = false)
        {
            builder.Services.Configure<AppInitializerOptions>(o =>
                o.Apps.Add(new App
                {
                    Id = appId,
                    DisplayName = displayName,
                    Languages = languages,
                    DefaultLanguage = defaultLanguage,
                    IsSystemApp = isSystem
                }));
            builder.Services.AddHostedService<AppInitializer>();

            return builder;
        }
    }
}
