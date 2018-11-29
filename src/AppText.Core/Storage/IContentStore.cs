using AppText.Core.ContentManagement;
using System.Threading.Tasks;

namespace AppText.Core.Storage
{
    public interface IContentStore
    {
        Task<ContentCollection[]> GetContentCollections(ContentCollectionQuery query);
        Task<string> AddContentCollection(ContentCollection contentCollection);
        Task UpdateContentCollection(ContentCollection contentCollection);
        Task DeleteContentCollection(string id);

        Task<ContentItem[]> GetContentItems(ContentItemQuery query);
        Task<ContentItem> GetContentItem(string id);
        Task<bool> ContentItemExists(string contentKey, string collectionId, string excludeId);
        Task<string> AddContentItem(ContentItem contentItem);
        Task UpdateContentItem(ContentItem contentItem);
        Task DeleteContentItem(string id);
    }
}