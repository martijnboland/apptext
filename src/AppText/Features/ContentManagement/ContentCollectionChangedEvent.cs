using AppText.Shared.Commands;

namespace AppText.Features.ContentManagement
{
    public class ContentCollectionChangedEvent : IEvent
    {
        public string AppId { get; set; }

        public string CollectionId { get; set; }

        public string Name { get; set; }

        public int Version { get; set; }
    }
}
