namespace AppText.Translations.Configuration
{
    /// <summary>
    /// Configuration options for AppText.Translations.
    /// </summary>
    public class AppTextTranslationsConfigurationOptions
    {
        /// <summary>
        /// Does the app require an authenticated user? When left empty, the possible value of the API configuration is used.
        /// </summary>
        public bool? RequireAuthenticatedUser { get; set; }

        /// <summary>
        /// A pre-defined authorization policy (name) that is applied to the API. When left empty, the possible value of the API configuration is used.
        /// </summary>
        public string RequiredAuthorizationPolicy { get; set; }
    }
}