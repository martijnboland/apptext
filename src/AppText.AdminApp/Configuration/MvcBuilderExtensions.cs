using AppText.Configuration;
using AppText.Shared.Infrastructure.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace AppText.AdminApp.Configuration
{
    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddAppTextAdmin(this IMvcCoreBuilder builder, Action<AppTextAdminConfigurationOptions> setupAction = null)
        {
            var assembly = typeof(Startup).Assembly;
            builder.AddApplicationPart(assembly);

            var options = GetOptions(builder.Services, setupAction);

            ConfigureServices(builder.Services, assembly, options);

            builder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix, assembly));
            });
            return builder;
        }

        public static IMvcBuilder AddAppTextAdmin(this IMvcBuilder builder, Action<AppTextAdminConfigurationOptions> setupAction = null)
        {
            var assembly = typeof(Startup).Assembly;
            builder.AddApplicationPart(assembly);

            var options = GetOptions(builder.Services, setupAction);

            ConfigureServices(builder.Services, assembly, options);

            builder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix, assembly));
            });
            return builder;
        }

        private static AppTextAdminConfigurationOptions GetOptions(
            IServiceCollection services,
            Action<AppTextAdminConfigurationOptions> setupAction = null
        )
        {
            var enrichOptions = setupAction ?? delegate { };
            var options = new AppTextAdminConfigurationOptions();

            enrichOptions(options);

            if (String.IsNullOrEmpty(options.RoutePrefix))
            {
                // No route prefix is set, try to get the route prefix from AppText (via intermediate service provider).
                var serviceProvider = services.BuildServiceProvider();
                var appTextConfiguration = serviceProvider.GetService<AppTextPublicConfiguration>();
                if (appTextConfiguration != null)
                {
                    // Configuration found. Set route prefix and API base url
                    options.RoutePrefix = appTextConfiguration.RoutePrefix;
                }
            }

            // Register options as singleton
            services.AddSingleton(options);

            return options;
        }

        private static void ConfigureServices(IServiceCollection services, Assembly assembly, AppTextAdminConfigurationOptions options)
        {
            // Register EmbeddedFileProvider for views
            services.Configure<RazorViewEngineOptions>(razorOptions =>
            {
                razorOptions.FileProviders.Add(new EmbeddedFileProvider(assembly));
            });

            // Register Embedded Static Files provider
            services.ConfigureOptions(typeof(EmbeddedStaticFilesOptions));
        }
    }
}
