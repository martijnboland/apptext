using Microsoft.Extensions.DependencyInjection;

namespace AppText.Configuration
{
    public class AppTextApiConfigurationOptions
    {
        public const string DefaultRoutePrefix = "cms";

        /// <summary>
        /// The route prefix for the API. Default is 'cms'.
        /// </summary>
        public string RoutePrefix { get; set; }

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

        /// <summary>
        /// Enable the interactive Graphql browser (located at /{RoutePrefix}/graphql/graphiql)
        /// </summary>
        public bool EnableGraphiql { get; set; }

        public AppTextApiConfigurationOptions(IServiceCollection services)
        {
            this.RoutePrefix = DefaultRoutePrefix;
            this.RequireAuthenticatedUser = false;
            this.RequiredAuthorizationPolicy = null;
            this.RegisterClaimsPrincipal = true;
            this.EnableGraphiql = false;
        }

        /// <summary>
        /// Returns a configuration DTO that can be stored or serialized and contains settings that could be interesting for other modules.
        /// </summary>
        /// <returns></returns>
        public AppTextPublicConfiguration ToPublicConfiguration()
        {
            return new AppTextPublicConfiguration
            {
                RoutePrefix = this.RoutePrefix,
                RequireAuthenticatedUser = this.RequireAuthenticatedUser,
                RequiredAuthorizationPolicy = this.RequiredAuthorizationPolicy
            };
        }
    }
}
