using AppText.Configuration;
using AppText.Shared.Infrastructure.Mvc;
using AppText.Translations.Controllers;
using AppText.Translations.Formatters;
using AppText.Translations.Initialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppText.Translations.Configuration
{
    public static class AppTextBuilderExtensions
    {
        public static AppTextBuilder AddTranslations(this AppTextBuilder appTextBuilder)
        {
            // Add translations initializer.
            appTextBuilder.Services.AddTransient<IStartupFilter>(sp => new TranslationsInitializer(sp));

            // Add controller routes
            var mvcBuilder = appTextBuilder.Services.AddMvcCore();
            var assembly = typeof(TranslationsController).Assembly;
            mvcBuilder.AddApplicationPart(assembly);

            // Try to find a route prefix for the AppText api. When found, use that one also for the controllers of this assembly. Otherwise leave empty.
            var serviceProvider = appTextBuilder.Services.BuildServiceProvider();
            var appTextConfiguration = serviceProvider.GetService<AppTextPublicConfiguration>();

            var translationsPrefix = appTextConfiguration != null ? appTextConfiguration.RoutePrefix : String.Empty;

            mvcBuilder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(translationsPrefix, assembly));
            });

            // Add the custom output formatters
            mvcBuilder.AddMvcOptions(options =>
            {
                options.OutputFormatters.Insert(0, new TranslationResultJsonFormatter());
                options.OutputFormatters.Add(new TranslationResultResxFormatter());
                options.OutputFormatters.Add(new TranslationResultPoFormatter());
            });
            mvcBuilder.AddFormatterMappings(options =>
            {
                options.SetMediaTypeMappingForFormat("resx", "text/microsoft-resx");
                options.SetMediaTypeMappingForFormat("po", "text/x-gettext-translation");
            });

            return appTextBuilder;
        }
    }
}
