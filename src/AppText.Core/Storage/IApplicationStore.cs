using AppText.Core.Application;

namespace AppText.Core.Storage
{
    public interface IApplicationStore
    {
        App[] GetApps(AppQuery query);
        App GetApp(string id);
        bool AppExists(string publicIdentifier, string id);
        string AddApp(App app);
        void UpdateApp(App app);
        void DeleteApp(string id);
    } 
}