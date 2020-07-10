using System.Reflection;

namespace AppText.Localization
{
    /// <summary>
    /// Options for the AppText.Localization add-on
    /// </summary>
    public class AppTextLocalizationOptions
    {
        /// <summary>
        /// Prefix localization keys with type or paths? (default true)
        /// </summary>
        public bool PrefixContentKeys { get; set; }

        /// <summary>
        /// Separator between content key prefix and content key (default '.')
        /// </summary>
        public string PrefixSeparator { get; set; }

        /// <summary>
        /// Create new empty content item with a key when the key is not found? (default false)
        /// </summary>
        public bool CreateItemsWhenNotFound { get; set; }

        /// <summary>
        /// AppText App id for which the content items should be stored. Default is the Assembly name of the entry assembly.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Default language. Note that when using an existing AppText app, this option has no effect.
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Should the ASP.NET Core request localization options (supported languages, default language) be based on the AppText app? (default false)
        /// </summary>
        public bool ConfigureRequestLocalizationOptions { get; set; }

        /// <summary>
        /// When set to true, the host application is terminated after changing the AppText app. This is required to immediately 
        /// see the effect of adding new languages or changing the default language. (default false)
        /// </summary>
        /// <remarks>
        /// WARNING: this only works when running behind a server like IIS that automatically restarts the process when it is terminated.
        /// Do not set this option to true when running standalone. It will kill your host application.
        /// </remarks>
        public bool RecycleHostAppAfterSavingApp { get; set; }

        /// <summary>
        /// The collection name where the content items are stored (default <see cref="Constants.DefaultCollectionName" />)
        /// </summary>
        public string CollectionName { get; set; }

        public AppTextLocalizationOptions()
        {
            PrefixContentKeys = true;
            PrefixSeparator = ".";
            CreateItemsWhenNotFound = false;
            AppId = Assembly.GetEntryAssembly().GetName().Name;
            DefaultLanguage = Constants.DefaultDefaultLanguage;
            CollectionName = Constants.DefaultCollectionName;
            ConfigureRequestLocalizationOptions = false;
            RecycleHostAppAfterSavingApp = false;
        }
    }
}
