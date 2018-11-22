using AppText.Core.Infrastructure;
using AppText.Core.Shared.Commands;
using AppText.Core.Shared.Queries;
using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using AppText.Core.Storage.LiteDb;
using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;
using System.Linq;

namespace AppText.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppText(this IServiceCollection services, AppTextConfigurationOptions options)
        {
            // ClaimsPrincipal
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddTransient(sp => sp.GetService<IHttpContextAccessor>().HttpContext.User);

            // Data store, TODO: refactor into proper configuration api
            if (options.ConnectionString == null)
            {
                throw new ArgumentException("The ConnectionString of the AppTextConfigurationOptions may not be null");
            }
            services.AddSingleton(sp => new LiteDatabase(options.ConnectionString));
            services.AddSingleton(sp => new LiteRepository(sp.GetRequiredService<LiteDatabase>()));
            services.AddScoped<IApplicationStore, ApplicationStore>();
            services.AddScoped<IContentDefinitionStore, ContentDefinitionStore>();
            services.AddScoped<IContentStore, ContentStore>();
            services.AddScoped<IVersioner, Versioner>();

            // Dispatcher
            services.AddScoped(serviceProvider => new Dispatcher(serviceProvider));

            // Command & Query handlers 
            var coreAssembly = typeof(Dispatcher).Assembly;

            services.Scan(s => s
                .FromAssemblies(coreAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services.Scan(s => s
                .FromAssemblies(coreAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            // Validators
            services.AddTransient(typeof(IValidator<>), typeof(Validator<>));
            services.Scan(s => s
                .FromAssemblies(coreAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                    .AsSelf()
                    .WithTransientLifetime());
            return services;
        }
    }
}
