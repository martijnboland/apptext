namespace AppText.Api.Infrastructure
{
    public class AppTextConfigurationOptions
    {
        public const string DefaultRoutePrefix = "cms";

        public AppTextConfigurationOptions()
        {
            this.RoutePrefix = DefaultRoutePrefix;
        }

        public string RoutePrefix { get; set; }

        public string ConnectionString { get; set; }
    }
}
