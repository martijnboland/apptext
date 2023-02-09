using AppText.Features.ContentManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Storage.EfCore
{
    public class ContentStore : IContentStore
    {
        private readonly AppTextDbContext _dbContext;

        public ContentStore(AppTextDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ContentCollection[]> GetContentCollections(ContentCollectionQuery query)
        {
            var q = _dbContext.ContentCollections.AsQueryable();
            if (!string.IsNullOrEmpty(query.AppId))
            {
                q = q.Where(cc => cc.AppId == query.AppId);
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(cc => cc.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                q = q.Where(cc => cc.Name == query.Name);
            }
            return await q.OrderBy(cc => cc.Name).ToArrayAsync();
        }

        public async Task<string> AddContentCollection(ContentCollection contentCollection)
        {
            contentCollection.Id = Guid.NewGuid().ToString();
            _dbContext.ContentCollections.Add(contentCollection);
            await _dbContext.SaveChangesAsync();
            return contentCollection.Id;
        }

        public async Task UpdateContentCollection(ContentCollection contentCollection)
        {
            _dbContext.ForceUpdate(contentCollection, cc => cc.Id == contentCollection.Id);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteContentCollection(string id, string appId)
        {
            var contentCollection = await _dbContext.ContentCollections.FindAsync(id);
            _dbContext.ContentCollections.Remove(contentCollection);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CollectionContainsContent(string collectionId, string appId)
        {
            return await _dbContext.ContentItems.AnyAsync(ci => ci.CollectionId == collectionId);
        }

        public async Task<ContentItem[]> GetContentItems(ContentItemQuery query)
        {
            var q = _dbContext.ContentItems.AsQueryable();
            if (!string.IsNullOrEmpty(query.AppId))
            {
                q = q.Where(ci => ci.AppId == query.AppId);
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(ci => ci.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.CollectionId))
            {
                q = q.Where(ci => ci.CollectionId == query.CollectionId);
            }
            if (!string.IsNullOrEmpty(query.ContentKey))
            {
                q = q.Where(ci => ci.ContentKey == query.ContentKey);
            }
            if (!string.IsNullOrEmpty(query.ContentKeyStartsWith))
            {
                q = q.Where(ci => ci.ContentKey.StartsWith(query.ContentKeyStartsWith));
            }

            switch (query.OrderBy)
            {
                case ContentItemQuery.ContentItemsOrderBy.LastModifiedAtDescending:
                    q = q.OrderByDescending(ci => ci.LastModifiedAt);
                    break;
                case ContentItemQuery.ContentItemsOrderBy.ContentKey:
                    q = q.OrderBy(ci => ci.ContentKey);
                    break;
            }
            
            if (query.Offset.HasValue)
            {
                q = q.Skip(query.Offset.Value);
            }
            if (query.First.HasValue)
            {
                q = q.Take(query.First.Value);
            }

            return await q.ToArrayAsync();
        }

        public async Task<ContentItem> GetContentItem(string id, string appId)
        {
            return await _dbContext.ContentItems.FindAsync(id);
        }

        public async Task<bool> ContentItemExists(string contentKey, string collectionId, string excludeId, string appId)
        {
            return await _dbContext.ContentItems.AnyAsync(ci => ci.CollectionId == collectionId && ci.ContentKey == contentKey && ci.Id != excludeId);
        }

        public async Task<string> AddContentItem(ContentItem contentItem)
        {
            contentItem.Id = Guid.NewGuid().ToString();
            _dbContext.ContentItems.Add(contentItem);
            await _dbContext.SaveChangesAsync();
            return contentItem.Id;
        }

        public async Task UpdateContentItem(ContentItem contentItem)
        {
            _dbContext.ForceUpdate(contentItem, ci => ci.Id == contentItem.Id);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteContentItem(string id, string appId)
        {
            var contentItem = await _dbContext.ContentItems.FindAsync(id);
            _dbContext.ContentItems.Remove(contentItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteContentItemsForCollection(string collectionId, string appId)
        {
            var contentItems = await _dbContext.ContentItems.Where(ci => ci.CollectionId == collectionId).ToArrayAsync();
            _dbContext.ContentItems.RemoveRange(contentItems);
            await _dbContext.SaveChangesAsync();
        }
    }
}