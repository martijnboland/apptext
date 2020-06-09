using Microsoft.Extensions.Localization.Internal;

namespace AppText.AdminApp
{
    public static class Constants
    {
        public const string AppTextAdminAppId = "apptext-admin";
        public const string AppTextAdminAppDescription = "AppText Admin App";
        public const string AppTextAdminLabelsCollection = "Labels";
        public const string AppTextAdminMessagesCollection = "Messages";
        public static readonly string[] AppTextAdminCollections = new[] { AppTextAdminLabelsCollection, AppTextAdminMessagesCollection };
    }
}
