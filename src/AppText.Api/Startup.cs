using System.IO;
using AppText.Core.Configuration;
using AppText.Core.Storage.NoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AppText.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ClaimsPrincipal
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddTransient(sp => sp.GetService<IHttpContextAccessor>().HttpContext.User);

            // AppText
            //var connectionString = $"FileName={Path.Combine(Env.ContentRootPath, "App_Data", "AppText.db")};Mode=Exclusive";
            var dataPath = Path.Combine(Env.ContentRootPath, "App_Data");
            services.AddAppText()
                .AddNoDbStorage(dataPath);
//                .AddLiteDbStorage(connectionString);

            // Mvc
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
