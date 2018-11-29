using AppText.Core.Application;
using System.Threading.Tasks;

namespace AppText.Core.Storage
{
    public interface IApplicationStore
    {
        Task<App[]> GetApps(AppQuery query);
        Task<App> GetApp(string id);
        Task<bool> AppExists(string id);
        Task<string> AddApp(App app);
        Task UpdateApp(App app);
        Task DeleteApp(string id);
    } 
}