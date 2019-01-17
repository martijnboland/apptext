using System;
using System.Linq;
using System.Threading.Tasks;
using AppText.Features.Application;
using NoDb;

namespace AppText.Storage.NoDb
{
    public class ApplicationStore : IApplicationStore
    {
        private const string ApplicationsProjectId = "apps";

        private readonly IBasicQueries<App> _queries;
        private readonly IBasicCommands<App> _commands;

        public ApplicationStore(IBasicQueries<App> queries, IBasicCommands<App> commands)
        {
            _queries = queries;
            _commands = commands;
        }

        public Task<App> GetApp(string id)
        {
            return _queries.FetchAsync(ApplicationsProjectId, id);
        }

        public async Task<App[]> GetApps(AppQuery query)
        {
            var apps = await _queries.GetAllAsync(ApplicationsProjectId);
            return apps.ToArray();
        }

        public async Task<bool> AppExists(string id)
        {
            var exists = (await _queries.GetAllAsync(ApplicationsProjectId)).Any(a => a.Id == id);
            return exists;
        }

        public async Task<string> AddApp(App app)
        {
            await _commands.CreateAsync(ApplicationsProjectId, app.Id, app);
            return app.Id;
        }

        public Task DeleteApp(string id)
        {
            return _commands.DeleteAsync(ApplicationsProjectId, id);
        }

        public Task UpdateApp(App app)
        {
            return _commands.UpdateAsync(ApplicationsProjectId, app.Id, app);
        }
    }
}
