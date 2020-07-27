using System;
using AppText.Features.GraphQL;
using AppText.Shared.Commands;
using AppText.Shared.Infrastructure;
using AppText.Shared.Infrastructure.Mvc;
using AppText.Shared.Infrastructure.Security;
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
    /// <summary>
    /// AppText components builder.
    /// </summary>
    public class AppTextBuilder
    {
        /// <summary>
        /// .NET Core DependencyInjection services collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// AppText public API configuration accessible for other AppText modules during configuration.
        /// </summary>
        public AppTextPublicApiConfiguration ApiConfiguration { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="AppTextBuilder"/> class.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptionsAction"></param>
        public AppTextBuilder(IServiceCollection services, Action<AppTextApiConfigurationOptions> configureOptionsAction = null)
        {
            Services = services;
            RegisterCoreServices();
            AddApi(configureOptionsAction);
        }

        private void RegisterCoreServices()
        {
            // Dispatcher
            Services.AddScoped<IDispatcher>(serviceProvider => new Dispatcher(serviceProvider));

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

        private void AddApi(Action<AppTextApiConfigurationOptions> configureOptionsAction = null)
        {
            var mvcBuilder = Services.AddMvcCore();
            var assembly = typeof(Startup).Assembly;
            mvcBuilder.AddApplicationPart(assembly);

            var options = GetOptions(Services, configureOptionsAction);

            mvcBuilder.AddMvcOptions(mvcOptions =>
            {
                mvcOptions.Conventions.Insert(0, new AppTextRouteConvention(options.RoutePrefix, assembly));
                mvcOptions.Conventions.Add(new AppTextAuthorizationConvention(options.RequireAuthenticatedUser, options.RequiredAuthorizationPolicy));
                mvcOptions.Conventions.Add(new AppTextGraphiqlConvention(options.EnableGraphiql));
                mvcOptions.Conventions.Add(new AppTextNewtonsoftJsonConvention(assembly));
            });
        }

        private AppTextApiConfigurationOptions GetOptions(
            IServiceCollection services,
            Action<AppTextApiConfigurationOptions> configureOptionsAction = null
        )
        {
            var enrichOptions = configureOptionsAction ?? delegate { };
            var options = new AppTextApiConfigurationOptions();
            enrichOptions(options);

            if (options.RegisterClaimsPrincipal)
            {
                RegisterClaimsPrincipal(services);
            }

            // Set API configuration from options.
            ApiConfiguration = options.ToApiConfiguration();

            return options;
        }

        private static void RegisterClaimsPrincipal(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddTransient(sp => sp.GetService<IHttpContextAccessor>().HttpContext.User);
        }
    }
}
