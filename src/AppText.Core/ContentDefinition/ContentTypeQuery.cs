using AppText.Core.Shared.Queries;
using AppText.Core.Storage;
using System.Threading.Tasks;

namespace AppText.Core.ContentDefinition
{
    public class ContentTypeQuery : IQuery<ContentType[]>
    {
        public string AppPublicId { get; set; }

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
