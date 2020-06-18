using AppText.Configuration;
using AppText.Features.ContentManagement;
using AppText.Localization.Initialization;
using AppText.Shared.Commands;
using AppText.Translations.Configuration;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;

namespace AppText.Localization
{
    public static class AppTextBuilderExtensions
    {
        public static void AddAppTextLocalization(this AppTextBuilder appTextBuilder, Action<AppTextLocalizationOptions> setupAction = null)
        {
            var services = appTextBuilder.Services;

            services.AddSingleton<IStringLocalizerFactory, AppTextStringLocalizerFactory>();
            services.AddSingleton<IHtmlLocalizerFactory, AppTextHtmlLocalizerFactory>();
            services.AddSingleton<AppTextBridge>();
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

            services.AddScoped<IEventHandler<ContentItemChangedEvent>, ContentItemChangedEventHandler>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            // Enable Translations module for our own translations.
            appTextBuilder.AddTranslations();

            // Add initializer as hosted service
            appTextBuilder.Services.AddHostedService<LocalizationInitializer>();
        }
    }
}
