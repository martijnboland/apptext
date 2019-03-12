using System.Collections.Generic;

namespace AppText.AdminApp.Configuration
{
    public class AppTextAdminConfigurationOptions
    {
        public const string DefaultRoutePrefix = "cmsadmin";

        /// <summary>
        /// The admin route prefix. When left empty, the possible route prefix of the API is used.
        /// </summary>
        public string RoutePrefix { get; set; }

        /// <summary>
        /// Base url of the API. When left empty, we're assuming that the API base url equals the admin base url (same host and route prefix). 
        /// </summary>
        public string ApiBaseUrl { get; set; }

        /// <summary>
        /// Does the app require an authenticated user? When left empty, the possible value of the API configuration is used. 
        /// </summary>
        public bool? RequireAuthenticatedUser { get; set; }

        /// <summary>
        /// A pre-defined authorization policy (name) that is applied to the API. When left empty, the possible value of the API configuration is used.
        /// </summary>
        public string RequiredAuthorizationPolicy { get; set; }

        /// <summary>
        /// What type of authentication to use for the admin app and accessing the API.
        /// </summary>
        public AppTextAdminAuthType AuthType { get; set; }

        /// <summary>
        /// Optional OpenID Connect settings (only applies when AuthType is set to Oidc).
        /// </summary>
        public Dictionary<string, string> OidcSettings { get; set; }

        public AppTextAdminConfigurationOptions()
        {
            this.RoutePrefix = DefaultRoutePrefix;
            this.OidcSettings = new Dictionary<string, string>();
        }
    }

    public enum AppTextAdminAuthType
    {
        /// <summary>
        /// Access API with (same-site) cookie authentication.
        /// </summary>
        DefaultCookie,

        /// <summary>
        /// Use an OpenID connect Identity Provider and access API with OAuth Bearer tokens.
        /// </summary>
        Oidc
    }
}
