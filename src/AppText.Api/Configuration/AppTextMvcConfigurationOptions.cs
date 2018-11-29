namespace AppText.Api.Configuration
{
    public class AppTextMvcConfigurationOptions
    {
        public const string DefaultRoutePrefix = "cms";

        public AppTextMvcConfigurationOptions()
        {
            this.RoutePrefix = DefaultRoutePrefix;
        }

        public string RoutePrefix { get; set; }
    }
}
