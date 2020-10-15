using AppText.Features.Application;
using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using AppText.Shared.Commands;
using System.Threading.Tasks;

namespace AppText.Features.GraphQL
{
    public class ClearSchemaEventHandlers :
        IEventHandler<AppChangedEvent>,
        IEventHandler<ContentTypeChangedEvent>,
        IEventHandler<ContentCollectionChangedEvent>,
        IEventHandler<ContentCollectionDeletedEvent>
    {
        private readonly SchemaResolver _schemaResolver;

        public ClearSchemaEventHandlers(SchemaResolver schemaResolver)
        {
            _schemaResolver = schemaResolver;
        }

        public Task Handle(AppChangedEvent publishedEvent)
        {
            _schemaResolver.Clear(publishedEvent.AppId);
            return Task.CompletedTask;
        }

        public Task Handle(ContentTypeChangedEvent publishedEvent)
        {
            _schemaResolver.Clear(publishedEvent.ContentType.AppId);
            return Task.CompletedTask;
        }

        public Task Handle(ContentCollectionChangedEvent publishedEvent)
        {
            _schemaResolver.Clear(publishedEvent.AppId);
            return Task.CompletedTask;
        }

        public Task Handle(ContentCollectionDeletedEvent publishedEvent)
        {
            _schemaResolver.Clear(publishedEvent.AppId);
            return Task.CompletedTask;
        }
    }
}
