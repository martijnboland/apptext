using AppText.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Features.Application
{
    public static class AppTextBuilderExtensions
    {
        public static AppTextBuilder InitializeApp(this AppTextBuilder builder, string appId, string displayName, string[] languages, string defaultLanguage)
        {
            builder.Services.AddTransient<IStartupFilter>(sp => new AppInitializer(sp, appId, displayName, languages, defaultLanguage));

            return builder;
        }
    }
}
