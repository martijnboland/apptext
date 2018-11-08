using AppText.Core.ContentManagement;

namespace AppText.Core.Storage
{
    public interface IContentStore
    {
        ContentItem[] GetContentItems(ContentItemQuery query);
        string AddContentItem(ContentItem contentItem);
        void UpdateContentItem(ContentItem contentItem);
        void DeleteContentItem(string id);
        ContentCollection[] GetContentCollections(ContentCollectionQuery query);
        string AddContentCollection(ContentCollection contentCollection);
        void UpdateContentCollection(ContentCollection contentCollection);
        void DeleteContentCollection(string id);
    }
}