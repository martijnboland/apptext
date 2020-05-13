using AppText.Features.ContentDefinition;
using AppText.Shared.Commands;
using AppText.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class ContentTypeChangedEventHandler : IEventHandler<ContentTypeChangedEvent>
    {
        private readonly IContentStore _contentStore;

        public ContentTypeChangedEventHandler(IContentStore contentStore)
        {
            _contentStore = contentStore;
        }

        public async Task Handle(ContentTypeChangedEvent publishedEvent)
        {
            // Check if there are collections that use this content type. If so, update the content type
            var allCollections = await _contentStore.GetContentCollections(new ContentCollectionQuery { AppId = publishedEvent.ContentType.AppId });
            var collectionsWithContentTypeAndOlderVersions = allCollections.Where(c =>
                c.ContentType.Id == publishedEvent.ContentType.Id && c.ContentType.Version < publishedEvent.ContentType.Version);
            foreach (var collection in collectionsWithContentTypeAndOlderVersions)
            {
                collection.ContentType = publishedEvent.ContentType;
                await _contentStore.UpdateContentCollection(collection);
            }
        }
    }
}
