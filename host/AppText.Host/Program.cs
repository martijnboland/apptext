using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AspNetHost = Microsoft.Extensions.Hosting.Host;

namespace AppText.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            AspNetHost.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
