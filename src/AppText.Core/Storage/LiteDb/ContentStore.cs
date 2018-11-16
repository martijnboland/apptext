using AppText.Core.ContentManagement;
using LiteDB;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AppText.Core.Storage.LiteDb
{
    public class ContentStore : IContentStore
    {
        private readonly LiteRepository _liteRepository;

        public ContentStore(LiteDatabase liteDatabase)
        {
            _liteRepository = new LiteRepository(liteDatabase);
        }

        public ContentCollection[] GetContentCollections(ContentCollectionQuery query)
        {
            var q = _liteRepository.Query<ContentCollection>();
            if (! string.IsNullOrEmpty(query.AppPublicId))
            {
                q = q.Where(cc => cc.ContentType.App.PublicId == query.AppPublicId);
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(cc => cc.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                q = q.Where(cc => cc.Name == query.Name);
            }
            return q.ToArray();
        }

        public string AddContentCollection(ContentCollection contentCollection)
        {
            contentCollection.Id = ObjectId.NewObjectId().ToString();
            return _liteRepository.Insert(contentCollection);
        }

        public void UpdateContentCollection(ContentCollection contentCollection)
        {
            _liteRepository.Update(contentCollection);
        }

        public void DeleteContentCollection(string id)
        {
            _liteRepository.Delete<ContentCollection>(id);
        }


        public ContentItem[] GetContentItems(ContentItemQuery query)
        {
            var q = _liteRepository.Query<ContentItem>();
            if (! string.IsNullOrEmpty(query.AppPublicId))
            {
                q = q.Where(ci => ci.App.PublicId == query.AppPublicId);
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(ci => ci.Id == query.Id);
            }
            if (! string.IsNullOrEmpty(query.CollectionId))
            {
                q = q.Where(ci => ci.CollectionId == query.CollectionId);
            }
            return q.ToArray();
        }

        public ContentItem GetContentItem(string id)
        {
            return _liteRepository.SingleById<ContentItem>(id);
        }

        public bool ContentItemExists(string contentKey, string collectionId, string excludeId)
        {
            return _liteRepository.Query<ContentItem>()
                .Where(ci => ci.CollectionId == collectionId && ci.ContentKey == contentKey && ci.Id != excludeId)
                .Exists();
        }

        public string AddContentItem(ContentItem contentItem)
        {
            contentItem.Id = ObjectId.NewObjectId().ToString();
            ConvertJObjectsToDictionaries(contentItem);
            return _liteRepository.Insert(contentItem);
        }

        public void UpdateContentItem(ContentItem contentItem)
        {
            ConvertJObjectsToDictionaries(contentItem);
            _liteRepository.Update(contentItem);
        }

        public void DeleteContentItem(string id)
        {
            _liteRepository.Delete<ContentItem>(id);
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
