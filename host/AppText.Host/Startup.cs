using System.IO;
using AppText.AdminApp.Configuration;
using AppText.Configuration;
using AppText.Host.Data;
using AppText.Host.Services;
using AppText.Storage.LiteDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppText.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var dataFolder = Configuration["DataFolder"];

            // Init
            services.AddHostedService<InitAdminUserHostedService>();

            // Auth
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite($"Data Source={Path.Combine(dataFolder, "Application.db")}"));
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AppText", policy => policy.RequireAuthenticatedUser());
            });

            // AppText configuration
            var connectionString = $"FileName={Path.Combine(dataFolder, "AppText.db")};Mode=Exclusive";

            services.AddAppText(options =>
            {
                options.RoutePrefix = "";
                options.EnableGraphiql = true;

                bool.TryParse(Configuration["DISABLE_AUTH"], out bool authDisabled);
                
                if (! authDisabled)
                {
                    options.RequiredAuthorizationPolicy = "AppText";
                }
            })
                .AddLiteDbStorage(connectionString)
                .AddAdmin();

            // MVC
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configuring AppText.Host with data folder {0}", Configuration["DataFolder"]);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
