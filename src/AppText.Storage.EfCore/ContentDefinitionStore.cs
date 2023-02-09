using AppText.Features.ContentDefinition;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Storage.EfCore
{
    public class ContentDefinitionStore : IContentDefinitionStore
    {
        private AppTextDbContext _dbContext;

        public ContentDefinitionStore(AppTextDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ContentType[]> GetContentTypes(ContentTypeQuery query)
        {
            var q = _dbContext.ContentTypes.AsQueryable();
            if (!string.IsNullOrEmpty(query.AppId))
            {
                if (query.IncludeGlobalContentTypes)
                {
                    q = q.Where(ct => ct.AppId == query.AppId || ct.AppId == null);
                }
                else
                {
                    q = q.Where(ct => ct.AppId == query.AppId);
                }
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(ct => ct.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                q = q.Where(ct => ct.Name == query.Name);
            }
            return await q.OrderByDescending(ct => ct.AppId).ThenBy(ct => ct.Name).ToArrayAsync();
        }


        public async Task<string> AddContentType(ContentType contentType)
        {
            contentType.Id = Guid.NewGuid().ToString();
            _dbContext.ContentTypes.Add(contentType);
            await _dbContext.SaveChangesAsync();
            return contentType.Id;
        }

        public async Task UpdateContentType(ContentType contentType)
        {
            _dbContext.ForceUpdate(contentType, ct => ct.Id == contentType.Id);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteContentType(string id, string appId)
        {
            var contentType = await _dbContext.ContentTypes.FindAsync(id);
            _dbContext.ContentTypes.Remove(contentType);
            await _dbContext.SaveChangesAsync();
        }
    }
}