using AppText.Core.Shared.Infrastructure;
using AppText.Core.Storage;
using AppText.Core.Storage.LiteDb;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Core.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static AppTextBuilder AddAppText(this IServiceCollection services)
        {
            return new AppTextBuilder(services);
        }
    }
}
