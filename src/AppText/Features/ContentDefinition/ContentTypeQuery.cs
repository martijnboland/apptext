using AppText.Shared.Queries;
using AppText.Storage;
using System.Threading.Tasks;

namespace AppText.Features.ContentDefinition
{
    public class ContentTypeQuery : IQuery<ContentType[]>
    {
        public string AppId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class ContentTypeQueryHandler : IQueryHandler<ContentTypeQuery, ContentType[]>
    {
        public IContentDefinitionStore _store;

        public ContentTypeQueryHandler(IContentDefinitionStore store)
        {
            _store = store;
        }

        public Task<ContentType[]> Handle(ContentTypeQuery query)
        {
            return _store.GetContentTypes(query);
        }
    }
}
