using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppText.Host.Services
{
    public class InitAdminUserHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<InitAdminUserHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public InitAdminUserHostedService(ILogger<InitAdminUserHostedService> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Check if admin user and admin password are set. If so, ensure that there is a user with those credentials.
            var adminUser = _configuration["AdminUser"];
            var adminPassword = _configuration["AdminPassword"];

            using (var serviceScope = _serviceProvider.CreateScope())
            using (var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>())
            {
                if (!string.IsNullOrEmpty(adminUser) && !String.IsNullOrEmpty(adminPassword))
                {
                    var user = await userManager.FindByNameAsync(adminUser);
                    IdentityResult result;
                    if (user == null)
                    {
                        user = new IdentityUser(adminUser);
                        result = await userManager.CreateAsync(user, adminPassword);
                    }
                    else
                    {
                        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                        result = await userManager.ResetPasswordAsync(user, resetToken, adminPassword);
                    }
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Successfully set admin user credentials on startup");
                    }
                    else
                    {
                        _logger.LogError("Setting admin user credentials on startup failed due to the following error(s):");
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError("Code: {0}, Error: {1}", error.Code, error.Description);
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
