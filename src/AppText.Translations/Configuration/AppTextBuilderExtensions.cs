using AppText.Configuration;
using AppText.Shared.Infrastructure.Mvc;
using AppText.Translations.Controllers;
using AppText.Translations.Formatters;
using AppText.Translations.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace AppText.Translations.Configuration
{
    public static class AppTextBuilderExtensions
    {
        public static AppTextBuilder AddTranslations(this AppTextBuilder appTextBuilder, Action<AppTextTranslationsConfigurationOptions> setupAction = null)
        {
            // First, check existence of TranslationsInitializer. If registered, we assume that this module already has been registered.
            if (appTextBuilder.Services.Any(s => s.ImplementationType == typeof(TranslationsInitializer)))
            {
                return appTextBuilder;
            }

            // Set options
            var enrichOptions = setupAction ?? delegate { };
            var options = new AppTextTranslationsConfigurationOptions();
            enrichOptions(options);

            var serviceProvider = appTextBuilder.Services.BuildServiceProvider();

            var appTextConfiguration = serviceProvider.GetService<AppTextPublicConfiguration>();
            
            // Try to inherit auth options from api configuration when not set
            if (!options.RequireAuthenticatedUser.HasValue)
            {
                options.RequireAuthenticatedUser = appTextConfiguration.RequireAuthenticatedUser;
            }
            if (options.RequiredAuthorizationPolicy == null)
            {
                options.RequiredAuthorizationPolicy = appTextConfiguration.RequiredAuthorizationPolicy;
            }

            // Add translations initializer.
            appTextBuilder.Services.AddHostedService<TranslationsInitializer>();

            // Add controller routes
            var mvcBuilder = appTextBuilder.Services.AddMvcCore();
            var assembly = typeof(TranslationsController).Assembly;
            mvcBuilder.AddApplicationPart(assembly);

            // Try to find a route prefix for the AppText api. When found, use that one also for the controllers of this assembly. Otherwise leave empty.
            var translationsPrefix = appTextConfiguration != null ? appTextConfiguration.RoutePrefix : String.Empty;

            mvcBuilder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(translationsPrefix, assembly));
                mvcOptions.Conventions.Add(new AppTextAuthorizationConvention(
                    options.RequireAuthenticatedUser.HasValue ? options.RequireAuthenticatedUser.Value : false,
                    options.RequiredAuthorizationPolicy,
                    assembly));
            });

            // Add the custom output formatters
            mvcBuilder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.OutputFormatters.Insert(0, new TranslationResultJsonFormatter());
                mvcOptions.OutputFormatters.Add(new TranslationResultResxFormatter());
                mvcOptions.OutputFormatters.Add(new TranslationResultPoFormatter());
            });
            mvcBuilder.AddFormatterMappings(formatterOptions =>
            {
                formatterOptions.SetMediaTypeMappingForFormat("resx", "text/microsoft-resx");
                formatterOptions.SetMediaTypeMappingForFormat("po", "text/x-gettext-translation");
            });

            return appTextBuilder;
        }
    }
}
