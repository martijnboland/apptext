using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppText.Features.ContentManagement;
using NoDb;

namespace AppText.Storage.NoDb
{
    public class ContentStore : IContentStore
    {
        private readonly IBasicQueries<ContentCollection> _contentColllectionQueries;
        private readonly IBasicCommands<ContentCollection> _contentCollectionCommands;
        private readonly IBasicQueries<ContentItem> _contentItemQueries;
        private readonly IBasicCommands<ContentItem> _contentItemCommands;

        public ContentStore(
            IBasicQueries<ContentCollection> contentColllectionQueries, 
            IBasicCommands<ContentCollection> contentCollectionCommands,
            IBasicQueries<ContentItem> contentItemQueries, 
            IBasicCommands<ContentItem> contentItemCommands)
        {
            _contentColllectionQueries = contentColllectionQueries;
            _contentCollectionCommands = contentCollectionCommands;
            _contentItemQueries = contentItemQueries;
            _contentItemCommands = contentItemCommands;
        }

        public async Task<ContentCollection[]> GetContentCollections(ContentCollectionQuery query)
        {
            if (String.IsNullOrEmpty(query.AppId))
            {
                throw new ArgumentException("NoDbStorage always requires the AppId property");
            }
            var contentCollections = await _contentColllectionQueries.GetAllAsync(query.AppId);
            if (!string.IsNullOrEmpty(query.Id))
            {
                contentCollections = contentCollections.Where(cc => cc.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                contentCollections = contentCollections.Where(cc => cc.Name == query.Name);
            }
            return contentCollections.ToArray();
        }

        public async Task<string> AddContentCollection(ContentCollection contentCollection)
        {
            contentCollection.Id = Guid.NewGuid().ToString();
            var projectId = contentCollection.ContentType.AppId;
            await _contentCollectionCommands.CreateAsync(projectId, contentCollection.Id, contentCollection);
            return contentCollection.Id;
        }

        public Task UpdateContentCollection(ContentCollection contentCollection)
        {
            return _contentCollectionCommands.UpdateAsync(contentCollection.ContentType.AppId, contentCollection.Id, contentCollection);
        }

        public Task DeleteContentCollection(string id, string appId)
        {
            return _contentCollectionCommands.DeleteAsync(appId, id);
        }

        public async Task<ContentItem[]> GetContentItems(ContentItemQuery query)
        {
            if (String.IsNullOrEmpty(query.AppId))
            {
                throw new ArgumentException("NoDbStorage always requires the AppId property");
            }
            var contentItems = await _contentItemQueries.GetAllAsync(query.AppId);
            if (!string.IsNullOrEmpty(query.Id))
            {
                contentItems = contentItems.Where(ci => ci.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.CollectionId))
            {
                contentItems = contentItems.Where(ci => ci.CollectionId == query.CollectionId);
            }
            if (! string.IsNullOrEmpty(query.ContentKey))
            {
                contentItems = contentItems.Where(ci => ci.ContentKey == query.ContentKey);
            }
            if (!string.IsNullOrEmpty(query.ContentKeyStartsWith))
            {
                contentItems = contentItems.Where(ci => ci.ContentKey.StartsWith(query.ContentKeyStartsWith));
            }

            contentItems = contentItems.OrderBy(ci => ci.ContentKey);

            if (query.Offset.HasValue)
            {
                contentItems = contentItems.Skip(query.Offset.Value);
            }
            if (query.First.HasValue)
            {
                contentItems = contentItems.Take(query.First.Value);
            }
            return contentItems.ToArray();
        }

        public Task<ContentItem> GetContentItem(string id, string appId)
        {
            return _contentItemQueries.FetchAsync(appId, id);
        }

        public async Task<bool> ContentItemExists(string contentKey, string collectionId, string excludeId, string appId)
        {
            var contentItems = await _contentItemQueries.GetAllAsync(appId);
            return contentItems.Any(ci => ci.ContentKey == contentKey && ci.CollectionId == collectionId && ci.Id != excludeId);
        }

        public async Task<string> AddContentItem(ContentItem contentItem)
        {
            contentItem.Id = Guid.NewGuid().ToString();
            await _contentItemCommands.CreateAsync(contentItem.AppId, contentItem.Id, contentItem);
            return contentItem.Id;
        }

        public Task UpdateContentItem(ContentItem contentItem)
        {
            return _contentItemCommands.UpdateAsync(contentItem.AppId, contentItem.Id, contentItem);
        }

        public Task DeleteContentItem(string id, string appId)
        {
            return _contentItemCommands.DeleteAsync(appId, id);
        }

        public async Task<bool> CollectionContainsContent(string collectionId, string appId)
        {
            var contentItems = await _contentItemQueries.GetAllAsync(appId);
            return contentItems.Any(ci => ci.CollectionId == collectionId);
        }
    }
}
