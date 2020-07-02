namespace AppText.Configuration
{
    /// <summary>
    /// Configuration settings object where settings are stored that need to be accessible for other modules. 
    /// </summary>
    public class AppTextPublicApiConfiguration
    {
        public string RoutePrefix { get; set; }
        public bool RequireAuthenticatedUser { get; set; }
        public string RequiredAuthorizationPolicy { get; set; }
    }
}
