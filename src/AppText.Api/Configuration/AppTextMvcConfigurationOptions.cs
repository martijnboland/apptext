using AppText.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Api.Configuration
{
    public class AppTextMvcConfigurationOptions
    {
        public const string DefaultRoutePrefix = "cms";

        public string RoutePrefix { get; set; }

        public AppTextBuilder AppTextServices { get; }

        public AppTextMvcConfigurationOptions(IServiceCollection services)
        {
            this.RoutePrefix = DefaultRoutePrefix;
            this.AppTextServices = services.AddAppText();
        }
    }
}
