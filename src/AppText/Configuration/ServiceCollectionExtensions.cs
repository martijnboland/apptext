using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppText.Configuration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds AppText Core components and API.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptionsAction"></param>
        /// <returns></returns>
        public static AppTextBuilder AddAppText(this IServiceCollection services, Action<AppTextApiConfigurationOptions> configureOptionsAction = null)
        {
            return new AppTextBuilder(services, configureOptionsAction);
        }
    }
}
