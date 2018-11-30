using AppText.Api.Infrastructure.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace AppText.Api.Configuration
{
    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddAppText(this IMvcCoreBuilder builder, Action<AppTextMvcConfigurationOptions> configureOptionsAction = null)
        {
            var options = GetOptions(builder.Services, configureOptionsAction);

            builder.AddApplicationPart(typeof(Startup).Assembly);
            builder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix));
                mvcOptions.Conventions.Add(new AppTextAuthorizationConvention(options.RequireAuthenticatedUser, options.RequiredAuthorizationPolicy));
            });
            return builder;
        }

        public static IMvcBuilder AddAppText(this IMvcBuilder builder, Action<AppTextMvcConfigurationOptions> configureOptionsAction = null)
        {
            var options = GetOptions(builder.Services, configureOptionsAction);

            builder.AddApplicationPart(typeof(Startup).Assembly);
            builder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix));
                mvcOptions.Conventions.Add(new AppTextAuthorizationConvention(options.RequireAuthenticatedUser, options.RequiredAuthorizationPolicy));
            });
            return builder;
        }

        private static AppTextMvcConfigurationOptions GetOptions(IServiceCollection services, Action<AppTextMvcConfigurationOptions> configureOptionsAction = null)
        {
            var enrichOptions = configureOptionsAction ?? delegate { };
            var options = new AppTextMvcConfigurationOptions(services);
            enrichOptions(options);

            if (options.RegisterClaimsPrincipal)
            {
                RegisterClaimsPrincipal(services);
            }

            return options;
        }

        private static void RegisterClaimsPrincipal(IServiceCollection services)
        {
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddTransient(sp => sp.GetService<IHttpContextAccessor>().HttpContext.User);
        }
    }
}
