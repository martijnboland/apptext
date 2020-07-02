using System.IO;
using AppText.Configuration;
using AppText.Features.Application;
using AppText.Storage.NoDb;
using AppText.Translations.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppText.Translations
{
    public class Startup
    {
        protected IHostEnvironment Env;
        public Startup(IHostEnvironment env)
        {
            Env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // AppText
            var dataPath = Path.Combine(Env.ContentRootPath, "App_Data");
            services.AddAppText()
                .AddNoDbStorage(dataPath)
                .AddTranslations()
                .InitializeApp("translations", "Translations Development App", new string[] { "en", "nl", "de", "fr" }, "en");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseEndpoints(config => config.MapControllers());
        }
    }
}
