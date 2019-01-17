using AppText.Core.ContentManagement;
using LiteDB;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Core.Storage.LiteDb
{
    public class ContentStore : IContentStore
    {
        private readonly LiteRepository _liteRepository;

        public ContentStore(LiteDatabase liteDatabase)
        {
            _liteRepository = new LiteRepository(liteDatabase);
        }

        public Task<ContentCollection[]> GetContentCollections(ContentCollectionQuery query)
        {
            var q = _liteRepository.Query<ContentCollection>();
            if (! string.IsNullOrEmpty(query.AppId))
            {
                q = q.Where(cc => cc.ContentType.AppId == query.AppId);
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(cc => cc.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                q = q.Where(cc => cc.Name == query.Name);
            }
            return Task.FromResult(q.ToArray());
        }

        public Task<string> AddContentCollection(ContentCollection contentCollection)
        {
            contentCollection.Id = ObjectId.NewObjectId().ToString();
            return Task.FromResult(_liteRepository.Insert(contentCollection).ToString());
        }

        public Task UpdateContentCollection(ContentCollection contentCollection)
        {
            _liteRepository.Update(contentCollection);
            return Task.CompletedTask;
        }

        public Task DeleteContentCollection(string id, string appId)
        {
            _liteRepository.Delete<ContentCollection>(id);
            return Task.CompletedTask;
        }

        public Task<ContentItem[]> GetContentItems(ContentItemQuery query)
        {
            var q = _liteRepository.Query<ContentItem>();
            if (! string.IsNullOrEmpty(query.AppId))
            {
                q = q.Where(ci => ci.AppId == query.AppId);
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(ci => ci.Id == query.Id);
            }
            if (! string.IsNullOrEmpty(query.CollectionId))
            {
                q = q.Where(ci => ci.CollectionId == query.CollectionId);
            }
            if (!string.IsNullOrEmpty(query.ContentKeyStartsWith))
            {
                q = q.Where(ci => ci.ContentKey.StartsWith(query.ContentKeyStartsWith));
            }
            if (query.Offset.HasValue)
            {
                q = q.Skip(query.Offset.Value);
            }
            if (query.First.HasValue)
            {
                q = q.Limit(query.First.Value);
            }

            return Task.FromResult(q.ToArray());
        }

        public Task<ContentItem> GetContentItem(string id, string appId)
        {
            return Task.FromResult(_liteRepository.SingleById<ContentItem>(id));
        }

        public Task<bool> ContentItemExists(string contentKey, string collectionId, string excludeId, string appId)
        {
            return Task.FromResult(_liteRepository.Query<ContentItem>()
                .Where(ci => ci.CollectionId == collectionId && ci.ContentKey == contentKey && ci.Id != excludeId)
                .Exists());
        }

        public Task<string> AddContentItem(ContentItem contentItem)
        {
            contentItem.Id = ObjectId.NewObjectId().ToString();
            ConvertJObjectsToDictionaries(contentItem);
            return Task.FromResult(_liteRepository.Insert(contentItem).ToString());
        }

        public Task UpdateContentItem(ContentItem contentItem)
        {
            ConvertJObjectsToDictionaries(contentItem);
            _liteRepository.Update(contentItem);
            return Task.CompletedTask;
        }

        public Task DeleteContentItem(string id, string appId)
        {
            _liteRepository.Delete<ContentItem>(id);
            return Task.CompletedTask;
        }

        private void ConvertJObjectsToDictionaries(ContentItem contentItem)
        {
            // Convert JObject instances to Dictionary<string, object>, so LiteDB can store these properly
            foreach (var contentPart in contentItem.Content.ToList())
            {
                if (contentPart.Value is JObject)
                {
                    contentItem.Content[contentPart.Key] = JObject.FromObject(contentPart.Value).ToObject<Dictionary<string, object>>();
                }
            }
        }
    }
}
