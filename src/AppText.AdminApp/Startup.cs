using AppText.AdminApp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AppText.Configuration;
using AppText.Storage.NoDb;
using System.IO;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace AppText.AdminApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dataPath = Path.Combine(Env.ContentRootPath, "App_Data");
            services.AddAppText(o =>
            {
                o.EnableGraphiql = true;
                o.RoutePrefix = "";
            })
                .AddNoDbStorage(dataPath)
                .AddAdmin(o =>
                {
                    o.EmbeddedViewsDisabled = true;
                });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFallbackToController("{*path:regex(^(?!dist).*$)}", "AppTextAdmin", "Admin");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.Options.DevServerPort = 5173;
                    spa.UseReactDevelopmentServer(npmScript: "start");
                });
            }
        }
    }
}
