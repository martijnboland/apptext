using AppText.Host.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using AspNetHost = Microsoft.Extensions.Hosting.Host;

namespace AppText.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            InitializeIdentityDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            AspNetHost.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void InitializeIdentityDatabase(IHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    services.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the identity database.");
                }
            }
        }
    }
}
