using AppText.Shared.Commands;
using System;

namespace AppText.Features.ContentManagement
{
    public class ContentItemChangedEvent : IEvent
    {
        public string AppId { get; set; }
        public string CollectionId { get; set; }
        public string ContentKey { get; set; }
        public int Version { get; set; }
        public DateTime TimeStamp { get; set; }
        public string ModifiedBy { get; set; }
    }
}
