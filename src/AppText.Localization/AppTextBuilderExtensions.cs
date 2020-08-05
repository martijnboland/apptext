using AppText.Configuration;
using AppText.Features.Application;
using AppText.Features.ContentManagement;
using AppText.Localization.Initialization;
using AppText.Shared.Commands;
using AppText.Shared.Infrastructure;
using AppText.Storage;
using AppText.Translations.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Linq;

namespace AppText.Localization
{
    public static class AppTextBuilderExtensions
    {
        /// <summary>
        /// Adds AppText as localization implementation for standard .NET Core Localization functionality. When added to an application, you can use the 
        /// standard .NET Core localization facilities (IStringLocalizer, IViewLocalizer), but with AppText as content backend.
        /// </summary>
        /// <param name="appTextBuilder">AppText components builder</param>
        /// <param name="setupAction">Configures AppText.Localization options</param>
        /// <returns></returns>
        public static AppTextBuilder AddAppTextLocalization(this AppTextBuilder appTextBuilder, Action<AppTextLocalizationOptions> setupAction = null)
        {
            var services = appTextBuilder.Services;

            services.AddSingleton<IStringLocalizerFactory, AppTextStringLocalizerFactory>();
            services.AddSingleton<IHtmlLocalizerFactory, AppTextHtmlLocalizerFactory>();
            services.AddSingleton<AppTextBridge>();
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

            services.AddScoped<IEventHandler<ContentItemChangedEvent>, ContentItemChangedEventHandler>();
            services.AddScoped<IEventHandler<AppChangedEvent>, AppChangedEventHandler>();

            // Register for IOptions<AppTextLocalizationOptions>
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            // Enable Translations module for our own translations.
            appTextBuilder.AddTranslations();

            // Add initializer as hosted service
            appTextBuilder.Services.AddHostedService<LocalizationInitializer>();

            // Configure RequestLocalizationOptions
            var enrichOptions = setupAction ?? delegate { };
            var options = new AppTextLocalizationOptions();
            enrichOptions(options);

            if (options.ConfigureRequestLocalizationOptions)
            {
                services.AddOptions<RequestLocalizationOptions>()
                    .Configure<IServiceScopeFactory>((locOptions, serviceScopeFactory) =>
                    {
                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var applicationStore = scope.ServiceProvider.GetRequiredService<IApplicationStore>();
                            var app = AsyncHelper.RunSync(() => applicationStore.GetApp(App.SanitizeAppId(options.AppId)));
                            locOptions.SupportedUICultures = app.Languages.Select(lang => new CultureInfo(lang)).ToList();
                            locOptions.DefaultRequestCulture = new RequestCulture(CultureInfo.CurrentCulture, new CultureInfo(app.DefaultLanguage));
                        }
                    });
            }

            return appTextBuilder;
        }
    }
}
