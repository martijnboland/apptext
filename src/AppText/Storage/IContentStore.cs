using AppText.Features.ContentManagement;
using System.Threading.Tasks;

namespace AppText.Storage
{
    public interface IContentStore
    {
        Task<ContentCollection[]> GetContentCollections(ContentCollectionQuery query);
        Task<string> AddContentCollection(ContentCollection contentCollection);
        Task UpdateContentCollection(ContentCollection contentCollection);
        Task DeleteContentCollection(string id, string appId);

        Task<ContentItem[]> GetContentItems(ContentItemQuery query);
        Task<ContentItem> GetContentItem(string id, string appId);
        Task<bool> ContentItemExists(string contentKey, string collectionId, string excludeId, string appId);
        Task<string> AddContentItem(ContentItem contentItem);
        Task UpdateContentItem(ContentItem contentItem);
        Task DeleteContentItem(string id, string appId);
        Task<bool> CollectionContainsContent(string collectionId, string appId);
        Task DeleteContentItemsForCollection(string collectionId, string appId);
    }
}