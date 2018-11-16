using AppText.Core.Application;
using LiteDB;

namespace AppText.Core.Storage.LiteDb
{
    public class ApplicationStore : IApplicationStore
    {
        private readonly LiteRepository _liteRepository;

        public ApplicationStore(LiteRepository liteRepository)
        {
            _liteRepository = liteRepository;
        }

        public App GetApp(string id)
        {
            return _liteRepository.SingleById<App>(id);
        }

        public App[] GetApps(AppQuery query)
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
            return q.ToArray();
        }

        public bool AppExists(string publicIdentifier, string id)
        {
            return _liteRepository.Query<App>()
               .Where(a => a.PublicId == publicIdentifier && a.Id != id)
               .Exists();
        }

        public string AddApp(App app)
        {
            app.Id = ObjectId.NewObjectId().ToString();
            return _liteRepository.Insert(app);
        }

        public void UpdateApp(App app)
        {
            _liteRepository.Update(app);
        }

        public void DeleteApp(string id)
        {
            _liteRepository.Delete<App>(id);
        }

    }
}
