using AppText.Core.ContentManagement;

namespace AppText.Core.Storage
{
    public interface IContentStore
    {
        ContentItem[] GetContentItems(ContentItemQuery query);
        string InsertContentItem(ContentItem contentItem);
        void UpdateContentItem(ContentItem contentItem);
        void DeleteContentItem(string id);
        ContentCollection[] GetContentCollections(ContentCollectionQuery query);
        string InsertContentCollection(ContentCollection contentCollection);
        void UpdateContentCollection(ContentCollection contentCollection);
        void DeleteContentCollection(string id);
    }
}