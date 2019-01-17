using Microsoft.Extensions.DependencyInjection;

namespace AppText.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static AppTextBuilder AddAppText(this IServiceCollection services)
        {
            return new AppTextBuilder(services);
        }
    }
}
