using AppText.Configuration;
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

            services.TryAddSingleton<IStringLocalizerFactory, AppTextStringLocalizerFactory>();
            services.TryAddSingleton<IHtmlLocalizerFactory, AppTextHtmlLocalizerFactory>();
            services.TryAddSingleton<AppTextBridge>();
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
        }
    }
}
