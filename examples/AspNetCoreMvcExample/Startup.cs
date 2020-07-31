using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppText.AdminApp.Configuration;
using AppText.Configuration;
using AppText.Features.Application;
using AppText.Localization;
using AppText.Storage.NoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMvcExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add AppText
            var dataPath = Path.Combine(Env.ContentRootPath, "App_Data");

            services.AddAppText()
                // Use simple JSON file storage
                .AddNoDbStorage(dataPath)
                // Admin api is available at /apptext
                .AddAdmin()
                // Initialize an app with languages and default language in AppText where the content is stored
                .InitializeApp("example_app", "ASP.NET Core example", new string[] { "en", "nl", "fr" }, "en")
                // Add AppText implementation for ASP.NET Core localization 
                .AddAppTextLocalization(options =>
                {
                    // The ID of the app that we created just before
                    options.AppId = "example_app";
                    // Create empty items in AppText for all keys that are not found
                    options.CreateItemsWhenNotFound = true;
                    // Use AppText app settings of example_app for ASP.NET Core request localization
                    // SupportedCultures becomes 'en', 'nl' and 'fr' and the DefaultCulture is 'en'
                    options.ConfigureRequestLocalizationOptions = true;
                });

            services.AddControllersWithViews()
                .AddViewLocalization();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

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
