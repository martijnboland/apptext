using AppText.Core.Application;
using LiteDB;
using System.Threading.Tasks;

namespace AppText.Core.Storage.LiteDb
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
            if (!string.IsNullOrEmpty(query.PublicId))
            {
                q = q.Where(a => a.PublicId == query.PublicId);
            }
            return Task.FromResult(q.ToArray());
        }

        public Task<bool> AppExists(string publicIdentifier, string id)
        {
            return Task.FromResult(_liteRepository.Query<App>()
               .Where(a => a.PublicId == publicIdentifier && a.Id != id)
               .Exists());
        }

        public Task<string> AddApp(App app)
        {
            app.Id = ObjectId.NewObjectId().ToString();
            return Task.FromResult(_liteRepository.Insert(app).ToString());
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

    }
}
