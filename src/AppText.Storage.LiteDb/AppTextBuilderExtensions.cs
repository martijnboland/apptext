using AppText.Configuration;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Storage.LiteDb
{
    public static class AppTextBuilderExtensions
    {
        public static void AddLiteDbStorage(this AppTextBuilder builder, string connectionString)
        {
            builder.Services.AddSingleton(sp => new LiteDatabase(connectionString));
            builder.Services.AddSingleton(sp => new LiteRepository(sp.GetRequiredService<LiteDatabase>()));

            builder.Services.AddScoped<IApplicationStore, ApplicationStore>();
            builder.Services.AddScoped<IContentDefinitionStore, ContentDefinitionStore>();
            builder.Services.AddScoped<IContentStore, ContentStore>();
            builder.Services.AddScoped<IVersioner, Versioner>();
        }
    }
}
