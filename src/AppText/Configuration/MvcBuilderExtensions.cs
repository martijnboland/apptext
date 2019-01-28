using AppText.Shared.Infrastructure.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace AppText.Configuration
{
    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddAppText(this IMvcCoreBuilder builder, Action<AppTextMvcConfigurationOptions> configureOptionsAction = null)
        {
            var assembly = typeof(Startup).Assembly;
            builder.AddApplicationPart(assembly);

            var options = GetOptions(builder.Services, configureOptionsAction);

            builder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix, assembly));
                mvcOptions.Conventions.Add(new AppTextAuthorizationConvention(options.RequireAuthenticatedUser, options.RequiredAuthorizationPolicy));
                mvcOptions.Conventions.Add(new AppTextGraphiqlConvention(options.EnableGraphiql));
            });
            return builder;
        }

        public static IMvcBuilder AddAppText(this IMvcBuilder builder, Action<AppTextMvcConfigurationOptions> configureOptionsAction = null)
        {
            var assembly = typeof(Startup).Assembly;
            builder.AddApplicationPart(assembly);

            var options = GetOptions(builder.Services, configureOptionsAction);

            builder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix, assembly));
                mvcOptions.Conventions.Add(new AppTextAuthorizationConvention(options.RequireAuthenticatedUser, options.RequiredAuthorizationPolicy));
                mvcOptions.Conventions.Add(new AppTextGraphiqlConvention(options.EnableGraphiql));
            });
            return builder;
        }

        private static AppTextMvcConfigurationOptions GetOptions(
            IServiceCollection services,
            Action<AppTextMvcConfigurationOptions> configureOptionsAction = null
        )
        {
            var enrichOptions = configureOptionsAction ?? delegate { };
            var options = new AppTextMvcConfigurationOptions(services);
            enrichOptions(options);

            if (options.RegisterClaimsPrincipal)
            {
                RegisterClaimsPrincipal(services);
            }

            // Register public options as singleton for other modules
            services.AddSingleton(options.ToPublicConfiguration());

            return options;
        }

        private static void RegisterClaimsPrincipal(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddTransient(sp => sp.GetService<IHttpContextAccessor>().HttpContext.User);
        }
    }
}
