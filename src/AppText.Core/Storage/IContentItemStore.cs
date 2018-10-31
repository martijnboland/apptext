using AppText.Core.ContentManagement;

namespace AppText.Core.Storage
{
    public interface IContentItemStore
    {
        string InsertContentItem(ContentItem contentItem);
        void UpdateContentItem(ContentItem contentItem);
        ContentItem[] GetContentItems(ContentItemQuery query);
    }
}