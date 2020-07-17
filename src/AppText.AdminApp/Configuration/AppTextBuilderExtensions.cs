using AppText.AdminApp.Controllers;
using AppText.AdminApp.Initialization;
using AppText.Configuration;
using AppText.Features.Application;
using AppText.Shared.Infrastructure.Mvc;
using AppText.Translations.Configuration;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace AppText.AdminApp.Configuration
{
    public static class AppTextBuilderExtensions
    {
        /// <summary>
        /// Adds the AppText admin UI. By default it uses the same virtual path as the API. It can be overridden via the options.
        /// </summary>
        /// <param name="appTextBuilder">AppText components builder</param>
        /// <param name="setupAction">Configures AppText.AdminApp options (optionally)</param>
        /// <returns></returns>
        public static AppTextBuilder AddAdmin(this AppTextBuilder appTextBuilder, Action<AppTextAdminConfigurationOptions> setupAction = null)
        {
            // Register as application part with embedded views
            var mvcBuilder = appTextBuilder.Services.AddMvcCore();
            var assembly = typeof(AdminController).Assembly;
            mvcBuilder.AddApplicationPart(assembly);

            var options = GetOptions(appTextBuilder.Services, appTextBuilder.ApiConfiguration, setupAction);

            ConfigureServices(appTextBuilder.Services, assembly, options);

            mvcBuilder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix, assembly));
                mvcOptions.Conventions.Add(new AppTextAuthorizationConvention(
                    options.RequireAuthenticatedUser.HasValue ? options.RequireAuthenticatedUser.Value : false,
                    options.RequiredAuthorizationPolicy,
                    assembly));
            });

            // Enable Translations module for our own translations.
            appTextBuilder.AddTranslations();

            // Create AdminApp
            appTextBuilder.InitializeApp(Constants.AppTextAdminAppId, Constants.AppTextAdminAppDescription, new[] { "en", "nl" }, "en", isSystem: true);

            // Import translations (register as IHostedService)
            appTextBuilder.Services.AddHostedService<AppTextAdminInitializer>();

            return appTextBuilder;
        }

        private static AppTextAdminConfigurationOptions GetOptions(
            IServiceCollection services,
            AppTextPublicApiConfiguration apiConfiguration,
            Action<AppTextAdminConfigurationOptions> setupAction = null
        )
        {
            var enrichOptions = setupAction ?? delegate { };
            var options = new AppTextAdminConfigurationOptions();

            enrichOptions(options);

            // Try to set empty options from AppText API configuration
            if (apiConfiguration != null)
            {
                if (String.IsNullOrEmpty(options.RoutePrefix))
                {
                    options.RoutePrefix = apiConfiguration.RoutePrefix;
                }
                if (!options.RequireAuthenticatedUser.HasValue)
                {
                    options.RequireAuthenticatedUser = apiConfiguration.RequireAuthenticatedUser;
                }
                if (options.RequiredAuthorizationPolicy == null)
                {
                    options.RequiredAuthorizationPolicy = apiConfiguration.RequiredAuthorizationPolicy;
                }
            }

            // Register options as singleton
            services.TryAddSingleton(options);

            return options;
        }

        private static void ConfigureServices(IServiceCollection services, Assembly assembly, AppTextAdminConfigurationOptions options)
        {
            if (! options.EmbeddedViewsDisabled)
            {
                // Register EmbeddedFileProvider for views
                services.Configure<MvcRazorRuntimeCompilationOptions>(razorOptions =>
                {
                    razorOptions.FileProviders.Add(new EmbeddedFileProvider(assembly));
                });

                // Register Embedded Static Files provider
                services.ConfigureOptions(typeof(EmbeddedStaticFilesOptions));
            }
        }
    }
}
