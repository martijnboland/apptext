using AppText.Api.Infrastructure.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
            });
            return builder;
        }

        private static AppTextMvcConfigurationOptions GetOptions(IServiceCollection services, Action<AppTextMvcConfigurationOptions> configureOptionsAction = null)
        {
            var enrichOptions = configureOptionsAction ?? delegate { };
            var options = new AppTextMvcConfigurationOptions(services);
            enrichOptions(options);

            return options;
        }
    }
}
