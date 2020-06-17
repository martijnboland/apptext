namespace AppText.Localization
{
    public class AppTextLocalizationOptions
    {
        public bool PrefixContentKeys { get; set; }

        public bool CreateItemsWhenNotFound { get; set; }

        public AppTextLocalizationOptions()
        {
            PrefixContentKeys = true;
            CreateItemsWhenNotFound = false;
        }
    }
}
