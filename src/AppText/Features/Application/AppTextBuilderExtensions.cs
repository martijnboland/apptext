using AppText.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Features.Application
{
    public static class AppTextBuilderExtensions
    {
        public static AppTextBuilder InitializeApp(this AppTextBuilder builder, string appId, string displayName, string[] languages, string defaultLanguage, bool isSystem = false)
        {
            builder.Services.AddHostedService(sp => new AppInitializer(sp, appId, displayName, languages, defaultLanguage, isSystem));

            return builder;
        }
    }
}
