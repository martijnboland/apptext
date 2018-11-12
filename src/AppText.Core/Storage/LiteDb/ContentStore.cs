using AppText.Core.ContentManagement;
using LiteDB;

namespace AppText.Core.Storage.LiteDb
{
    public class ContentStore : IContentStore
    {
        private readonly LiteRepository _liteRepository;

        public ContentStore(LiteDatabase liteDatabase)
        {
            _liteRepository = new LiteRepository(liteDatabase);            
        }

        public ContentItem[] GetContentItems(ContentItemQuery query)
        {
            var q = _liteRepository.Query<ContentItem>();
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

        public string AddContentItem(ContentItem contentItem)
        {
            contentItem.Id = ObjectId.NewObjectId().ToString();
            return _liteRepository.Insert(contentItem);
        }

        public void UpdateContentItem(ContentItem contentItem)
        {
            _liteRepository.Update(contentItem);
        }

        public void DeleteContentItem(string id)
        {
            _liteRepository.Delete<ContentItem>(id);
        }

        public ContentCollection[] GetContentCollections(ContentCollectionQuery query)
        {
            var q = _liteRepository.Query<ContentCollection>();
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
    }
}
