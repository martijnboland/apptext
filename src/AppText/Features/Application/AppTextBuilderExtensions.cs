using AppText.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Features.Application
{
    public static class AppTextBuilderExtensions
    {
        /// <summary>
        /// Initialize an app in AppText from code.
        /// </summary>
        /// <param name="builder">Apptext components builder</param>
        /// <param name="appId">The App identifier (no spaces)</param>
        /// <param name="displayName">Display name of the app</param>
        /// <param name="languages">Array with available languages</param>
        /// <param name="defaultLanguage">Default language</param>
        /// <param name="isSystem">Indicates if the app should function as a system app. This prevents accidental removal for example.</param>
        /// <returns></returns>
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
