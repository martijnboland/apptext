using AppText.Shared.Commands;

namespace AppText.Features.ContentDefinition
{
    public class ContentTypeChangedEvent : IEvent
    {
        public ContentType ContentType { get; set; }
    }
}
