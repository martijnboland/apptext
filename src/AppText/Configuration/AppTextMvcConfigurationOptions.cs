using Microsoft.Extensions.DependencyInjection;

namespace AppText.Configuration
{
    public class AppTextMvcConfigurationOptions
    {
        public const string DefaultRoutePrefix = "cms";

        /// <summary>
        /// The route prefix for the API. Default is 'cms'.
        /// </summary>
        public string RoutePrefix { get; set; }

        /// <summary>
        /// Extension point to register additional services.
        /// </summary>
        public AppTextBuilder AppTextServices { get; }

        /// <summary>
        /// Does the API require an authenticated user?
        /// </summary>
        public bool RequireAuthenticatedUser { get; set; }

        /// <summary>
        /// A pre-defined authorization policy (name) that is applied to the API.
        /// </summary>
        public string RequiredAuthorizationPolicy { get; set; }

        /// <summary>
        /// Should HttpContext.User be registered as ClaimsPrincipal? (default true).
        /// </summary>
        public bool RegisterClaimsPrincipal { get; set; }

        public AppTextMvcConfigurationOptions(IServiceCollection services)
        {
            this.RoutePrefix = DefaultRoutePrefix;
            this.AppTextServices = services.AddAppText();
            this.RequireAuthenticatedUser = false;
            this.RequiredAuthorizationPolicy = null;
            this.RegisterClaimsPrincipal = true;
        }
    }
}
