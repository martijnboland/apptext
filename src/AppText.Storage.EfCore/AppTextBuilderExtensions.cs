using AppText.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppText.Storage.EfCore
{
    public static class AppTextBuilderExtensions
    {
        /// <summary>
        /// Adds EF Core storage for AppText
        /// </summary>
        /// <param name="builder">The AppText services builder</param>
        /// <param name="optionsAction">EF Core options action</param>
        /// <param name="contextLifetime">EF Core DbContext lifetime</param>
        /// <param name="optionsLifetime">EF Core DbContextOptions lifetime</param>
        /// <returns></returns>
        public static AppTextBuilder AddEfCoreDbStorage(
            this AppTextBuilder builder, 
            Action<DbContextOptionsBuilder> optionsAction = null, 
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            builder.Services.AddDbContext<AppTextDbContext>(optionsAction: optionsAction, contextLifetime: contextLifetime, optionsLifetime: optionsLifetime);

            builder.Services.AddScoped<IApplicationStore, ApplicationStore>();
            builder.Services.AddScoped<IContentDefinitionStore, ContentDefinitionStore>();
            builder.Services.AddScoped<IContentStore, ContentStore>();
            builder.Services.AddScoped<IVersioner, Versioner>();

            return builder;
        }
    }
}
