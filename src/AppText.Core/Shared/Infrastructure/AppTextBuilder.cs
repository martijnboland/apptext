using AppText.Core.Shared.Commands;
using AppText.Core.Shared.Queries;
using AppText.Core.Shared.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Core.Shared.Infrastructure
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
        }
    }
}
