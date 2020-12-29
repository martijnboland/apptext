using System.IO;
using AppText.AdminApp.Configuration;
using AppText.Configuration;
using AppText.Host.Services;
using AppText.Storage.LiteDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LiteDB.Identity.Extensions;
using LiteDB.Identity.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace AppText.Host
{
    public class Startup
    {
        private const string API_VERSION = "v1";

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
            services.AddLiteDBIdentity($"Filename={Path.Combine(dataFolder, "Identity.db")};Mode=Exclusive")
                .AddUserManager<UserManager<LiteDbUser>>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            var corsOrigins = Configuration["CorsOrigins"].Split(new [] { ',',';' });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(corsOrigins);
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AppText", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
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

            // Swagger/OpenAPI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(API_VERSION, new OpenApiInfo
                {
                    Title = "AppText API",
                    Version = API_VERSION
                });
            });
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{API_VERSION}/swagger.json", $"AppText API {API_VERSION}");
                c.DocumentTitle = "AppText API Swagger";
            });

            app.UseRouting();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            bool.TryParse(Configuration["DISABLE_DELETE"], out bool deleteDisabled);

            if (deleteDisabled)
            {
                app.Use(async (context, next) =>
                {
                    if (context.Request.Method == HttpMethod.Delete.ToString())
                    {
                        context.Response.StatusCode = 405;
                        await context.Response.WriteAsync("DELETE Method is not allowed");
                    }
                    else
                    {
                        await next.Invoke();
                    }
                });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
