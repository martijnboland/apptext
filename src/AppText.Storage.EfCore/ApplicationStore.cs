using AppText.Features.Application;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Storage.EfCore
{
    public class ApplicationStore : IApplicationStore
    {
        private readonly AppTextDbContext _dbContext;

        public ApplicationStore(AppTextDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddApiKey(ApiKey apiKey)
        {
            apiKey.Id = Guid.NewGuid().ToString();
            _dbContext.ApiKeys.Add(apiKey);
            await _dbContext.SaveChangesAsync();
            return apiKey.Id;
        }

        public async Task<string> AddApp(App app)
        {
            _dbContext.Apps.Add(app);
            await _dbContext.SaveChangesAsync();
            return app.Id;
        }

        public async Task<bool> AppExists(string id)
        {
            return await _dbContext.Apps.AnyAsync(a => a.Id == id);
        }

        public async Task DeleteApiKey(string id, string appId)
        {
            var apiKey = await _dbContext.ApiKeys.FindAsync(id);
            _dbContext.ApiKeys.Remove(apiKey);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteApp(string id)
        {
            var app = await _dbContext.Apps.FindAsync(id);
            _dbContext.Apps.Remove(app);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiKey[]> GetApiKeys(ApiKeysQuery query)
        {
            var q = _dbContext.ApiKeys.AsQueryable();

            if (!string.IsNullOrEmpty(query.AppId))
            {
                q = q.Where(a => a.AppId == query.AppId);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                q = q.Where(a => a.Name == query.Name);
            }
            if (!string.IsNullOrEmpty(query.Key))
            {
                q = q.Where(a => a.Key == query.Key);
            }
            return await q.OrderBy(a => a.Name).ToArrayAsync();
        }

        public async Task<App> GetApp(string id)
        {
            return await _dbContext.Apps.FindAsync(id);
        }

        public async Task<App[]> GetApps(AppQuery query)
        {
            var q = _dbContext.Apps.AsQueryable();
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(a => a.Id == query.Id);
            }
            return await q.OrderBy(a => a.IsSystemApp).ToArrayAsync();
        }

        public async Task UpdateApp(App app)
        {
            _dbContext.Apps.Update(app);
            await _dbContext.SaveChangesAsync();
        }
    }
}