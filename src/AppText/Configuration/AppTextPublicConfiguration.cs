namespace AppText.Configuration
{
    /// <summary>
    /// Configuration settings object where settings are stored that need to be accessible for other modules. 
    /// </summary>
    public class AppTextPublicApiConfiguration
    {
        /// <summary>
        /// The route prefix for the API. Default is 'apptext'.
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
    }
}
