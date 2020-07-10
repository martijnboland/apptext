using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppText.Configuration
{
    /// <summary>
    /// AppText extension methods for IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds AppText Core components and API.
        /// </summary>
        /// <param name="services">ServiceCollection where all available components are registered for DependencyInjection</param>
        /// <param name="configureOptionsAction">Configure AppText API options with this action</param>
        /// <returns>The AppTextBuilder instance. This is used to add extensions to AppText</returns>
        public static AppTextBuilder AddAppText(this IServiceCollection services, Action<AppTextApiConfigurationOptions> configureOptionsAction = null)
        {
            return new AppTextBuilder(services, configureOptionsAction);
        }
    }
}
