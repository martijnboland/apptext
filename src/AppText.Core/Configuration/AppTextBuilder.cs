using System;
using AppText.Core.GraphQL;
using AppText.Core.Shared.Commands;
using AppText.Core.Shared.Infrastructure;
using AppText.Core.Shared.Queries;
using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using GraphQL;
using GraphQL.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Core.Configuration
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
            Services.AddSingleton<IScopedServiceFactory, HttpContextScopedServiceFactory>();
            Services.AddSingleton<Func<IApplicationStore>>(sp => 
                () => sp.GetRequiredService<IScopedServiceFactory>().GetService<IApplicationStore>());
            Services.AddSingleton<Func<IContentStore>>(sp => 
                () => sp.GetRequiredService<IScopedServiceFactory>().GetService<IContentStore>());
        }
    }
}
