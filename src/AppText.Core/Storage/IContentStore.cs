using AppText.Core.ContentManagement;

namespace AppText.Core.Storage
{
    public interface IContentStore
    {
        ContentCollection[] GetContentCollections(ContentCollectionQuery query);
        string AddContentCollection(ContentCollection contentCollection);
        void UpdateContentCollection(ContentCollection contentCollection);
        void DeleteContentCollection(string id);

        ContentItem[] GetContentItems(ContentItemQuery query);
        ContentItem GetContentItem(string id);
        bool ContentItemExists(string contentKey, string collectionId, string id);
        string AddContentItem(ContentItem contentItem);
        void UpdateContentItem(ContentItem contentItem);
        void DeleteContentItem(string id);
    }
}