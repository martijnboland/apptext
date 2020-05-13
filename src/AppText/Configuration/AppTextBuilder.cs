using System;
using AppText.Features.GraphQL;
using AppText.Shared.Commands;
using AppText.Shared.Infrastructure;
using AppText.Shared.Queries;
using AppText.Shared.Validation;
using AppText.Storage;
using GraphQL;
using GraphQL.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppText.Configuration
{
    public class AppTextBuilder
    {
        public IServiceCollection Services { get; }

        public AppTextBuilder(IServiceCollection services)
        {
            Services = services;
            RegisterCoreServices();
        }

        private void RegisterCoreServices()
        {
            // Dispatcher
            Services.AddScoped(serviceProvider => new Dispatcher(serviceProvider));

            // Command & Query handlers 
            var coreAssembly = this.GetType().Assembly;

            Services.Scan(s => s
                .FromAssemblies(coreAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            Services.Scan(s => s
                .FromAssemblies(coreAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            Services.Scan(s => s
                .FromAssemblies(coreAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            // Validators
            Services.AddTransient(typeof(IValidator<>), typeof(Validator<>));
            Services.Scan(s => s
                .FromAssemblies(coreAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                    .AsSelf()
                    .WithTransientLifetime());

            // Graphql
            Services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            Services.AddSingleton<IDocumentWriter, DocumentWriter>();
            Services.AddSingleton<SchemaResolver>();

            // Store factories
            Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.AddSingleton<IScopedServiceFactory, HttpContextScopedServiceFactory>();

            Services.AddSingleton<Func<IApplicationStore>>(sp => 
                () => sp.GetRequiredService<IScopedServiceFactory>().GetService<IApplicationStore>());
            Services.AddSingleton<Func<IContentStore>>(sp => 
                () => sp.GetRequiredService<IScopedServiceFactory>().GetService<IContentStore>());

            // Cache
            Services.AddMemoryCache();
        }
    }
}
