using System;
using System.Linq;
using System.Threading.Tasks;
using AppText.Features.Application;
using NoDb;

namespace AppText.Storage.NoDb
{
    public class ApplicationStore : IApplicationStore
    {
        private readonly IBasicQueries<App> _appQueries;
        private readonly IBasicCommands<App> _appCommands;
        private readonly IBasicQueries<ApiKey> _apiKeyQueries;
        private readonly IBasicCommands<ApiKey> _apiKeyCommands;


        public ApplicationStore(IBasicQueries<App> appQueries, IBasicCommands<App> appCommands, IBasicQueries<ApiKey> apiKeyQueries, IBasicCommands<ApiKey> apiKeyCommands)
        {
            _appQueries = appQueries;
            _appCommands = appCommands;
            _apiKeyQueries = apiKeyQueries;
            _apiKeyCommands = apiKeyCommands;
        }

        public Task<App> GetApp(string id)
        {
            return _appQueries.FetchAsync(Constants.ApplicationsProjectId, id);
        }

        public async Task<App[]> GetApps(AppQuery query)
        {
            var apps = await _appQueries.GetAllAsync(Constants.ApplicationsProjectId);
            return apps.OrderBy(a => a.IsSystemApp).ThenBy(a => a.Id).ToArray();
        }

        public async Task<bool> AppExists(string id)
        {
            var exists = (await _appQueries.GetAllAsync(Constants.ApplicationsProjectId)).Any(a => a.Id == id);
            return exists;
        }

        public async Task<string> AddApp(App app)
        {
            await _appCommands.CreateAsync(Constants.ApplicationsProjectId, app.Id, app);
            return app.Id;
        }

        public Task DeleteApp(string id)
        {
            return _appCommands.DeleteAsync(Constants.ApplicationsProjectId, id);
        }

        public Task UpdateApp(App app)
        {
            return _appCommands.UpdateAsync(Constants.ApplicationsProjectId, app.Id, app);
        }

        public async Task<string> AddApiKey(ApiKey apiKey)
        {
            apiKey.Id = Guid.NewGuid().ToString();
            await _apiKeyCommands.CreateAsync(apiKey.AppId, apiKey.Id, apiKey);
            return apiKey.Id;
        }

        public async Task<ApiKey[]> GetApiKeys(ApiKeysQuery query)
        {
            if (String.IsNullOrEmpty(query.AppId))
            {
                throw new ArgumentException("NoDbStorage always requires the AppId property");
            }
            var apiKeys = await _apiKeyQueries.GetAllAsync(query.AppId);

            if (! string.IsNullOrEmpty(query.Name))
            {
                apiKeys = apiKeys.Where(a => a.Name == query.Name);
            }
            if (!string.IsNullOrEmpty(query.Key))
            {
                apiKeys = apiKeys.Where(a => a.Key == query.Key);
            }
            return apiKeys.OrderBy(a => a.Name).ToArray();
        }

        public async Task DeleteApiKey(string id, string appId)
        {
            await _apiKeyCommands.DeleteAsync(appId, id);
        }
    }
}
