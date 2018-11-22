using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppText.Api.Infrastructure
{
    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddAppText(this IMvcCoreBuilder builder, Action<AppTextConfigurationOptions> configureOptionsAction = null)
        {
            var options = GetOptions(configureOptionsAction);

            builder.AddApplicationPart(typeof(Startup).Assembly);
            builder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix));
            });
            builder.Services.AddAppText(options);
            return builder;
        }

        public static IMvcBuilder AddAppText(this IMvcBuilder builder, Action<AppTextConfigurationOptions> configureOptionsAction = null)
        {
            var options = GetOptions(configureOptionsAction);

            builder.AddApplicationPart(typeof(Startup).Assembly);
            builder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix));
            });
            builder.Services.AddAppText(options);
            return builder;
        }

        private static AppTextConfigurationOptions GetOptions(Action<AppTextConfigurationOptions> configureOptionsAction = null)
        {
            var enrichOptions = configureOptionsAction ?? delegate { };
            var options = new AppTextConfigurationOptions();
            enrichOptions(options);

            return options;
        }
    }
}
