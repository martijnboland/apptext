using AppText.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RunMethodsSequentially;
using System;

namespace AppText.Storage.EfCore
{
    public static class AppTextBuilderExtensions
    {
        /// <summary>
        /// Adds EF Core storage for AppText
        /// </summary>
        /// <param name="builder">The AppText services builder</param>
        /// <param name="providerType">Database provider (SQL Server, Postgres, etc)</param>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        public static AppTextBuilder AddEfCoreDbStorage(this AppTextBuilder builder, ProviderType providerType, string connectionString)
        {
            // Register services
            Action<DbContextOptionsBuilder> optionsAction = null;
            switch (providerType)
            {
                case ProviderType.SqlServer:
                    optionsAction = options =>
                    {
                        options.UseSqlServer(connectionString);
                    };
                    break;
                default:
                    throw new NotImplementedException($"Provider {providerType} is not implemented yet.");
            }
            builder.Services.AddDbContext<AppTextDbContext>(optionsAction: optionsAction);

            builder.Services.AddScoped<IApplicationStore, ApplicationStore>();
            builder.Services.AddScoped<IContentDefinitionStore, ContentDefinitionStore>();
            builder.Services.AddScoped<IContentStore, ContentStore>();
            builder.Services.AddScoped<IVersioner, Versioner>();

            // Register startup services
            builder.Services.RegisterRunMethodsSequentially(options =>
            {
                switch (providerType)
                {
                    case ProviderType.SqlServer:
                        options.AddSqlServerLockAndRunMethods(connectionString);
                        break;
                }
            })
                .RegisterServiceToRunInJob<AppTextDbContextInitializer>();
            
            return builder;
        }
    }
}
