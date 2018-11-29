using AppText.Core.Shared.Queries;
using AppText.Core.Storage;
using System.Threading.Tasks;

namespace AppText.Core.ContentManagement
{
    public class ContentCollectionQuery : IQuery<ContentCollection[]>
    {
        public string AppId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ContentCollectionQueryHandler : IQueryHandler<ContentCollectionQuery, ContentCollection[]>
    {
        private readonly IContentStore _contentItemStore;

        public ContentCollectionQueryHandler(IContentStore contentItemStore)
        {
            _contentItemStore = contentItemStore;
        }

        public Task<ContentCollection[]> Handle(ContentCollectionQuery query)
        {
            return _contentItemStore.GetContentCollections(query);
        }
    }
}
