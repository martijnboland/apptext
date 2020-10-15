using AppText.Shared.Commands;

namespace AppText.Features.ContentManagement
{
    public class ContentCollectionDeletedEvent : IEvent
    {
        public string AppId { get; set; }
        public string CollectionId { get; set; }
    }
}
