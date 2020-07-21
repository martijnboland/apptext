using AppText.Features.Application;
using LiteDB;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Storage.LiteDb
{
    public class ApplicationStore : IApplicationStore
    {
        private readonly LiteRepository _liteRepository;

        public ApplicationStore(LiteRepository liteRepository)
        {
            _liteRepository = liteRepository;
        }

        public Task<App> GetApp(string id)
        {
            return Task.FromResult(_liteRepository.SingleById<App>(id));
        }

        public Task<App[]> GetApps(AppQuery query)
        {
            var q = _liteRepository.Query<App>();
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(a => a.Id == query.Id);
            }
            return Task.FromResult(q.OrderBy(a => a.IsSystemApp).ToArray());
        }

        public Task<bool> AppExists(string id)
        {
            return Task.FromResult(_liteRepository.Query<App>()
               .Where(a => a.Id == id)
               .Exists());
        }

        public Task<string> AddApp(App app)
        {
            _liteRepository.Insert(app);
            return Task.FromResult(app.Id);
        }

        public Task UpdateApp(App app)
        {
            _liteRepository.Update(app);
            return Task.CompletedTask;
        }

        public Task DeleteApp(string id)
        {
            _liteRepository.Delete<App>(id);
            return Task.CompletedTask;
        }

        public Task<string> AddApiKey(ApiKey apiKey)
        {
            _liteRepository.Insert(apiKey);
            return Task.FromResult(apiKey.Id);
        }

        public Task<ApiKey[]> GetApiKeys(ApiKeysQuery query)
        {
            var q = _liteRepository.Query<ApiKey>();

            if (!string.IsNullOrEmpty(query.AppId))
            {
                q = q.Where(a => a.AppId == query.AppId);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                q = q.Where(a => a.Name == query.Name);
            }
            return Task.FromResult(q.OrderBy(a => a.Name).ToArray());
        }

        public Task DeleteApiKey(string id, string appId)
        {
            _liteRepository.Delete<ApiKey>(id);
            return Task.CompletedTask;
        }
    }
}
