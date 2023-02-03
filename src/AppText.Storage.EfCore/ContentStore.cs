using AppText.Features.ContentManagement;
using System.Threading.Tasks;

namespace AppText.Storage.EfCore
{
    public class ContentStore : IContentStore
    {
        public Task<string> AddContentCollection(ContentCollection contentCollection)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> AddContentItem(ContentItem contentItem)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CollectionContainsContent(string collectionId, string appId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ContentItemExists(string contentKey, string collectionId, string excludeId, string appId)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteContentCollection(string id, string appId)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteContentItem(string id, string appId)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteContentItemsForCollection(string collectionId, string appId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ContentCollection[]> GetContentCollections(ContentCollectionQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task<ContentItem> GetContentItem(string id, string appId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ContentItem[]> GetContentItems(ContentItemQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateContentCollection(ContentCollection contentCollection)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateContentItem(ContentItem contentItem)
        {
            throw new System.NotImplementedException();
        }
    }
}