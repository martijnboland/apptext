using AppText.Shared.Queries;
using AppText.Storage;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class ContentItemQuery : IQuery<ContentItem[]>
    {
        public string AppId { get; set; }
        public string Id { get; set; }
        public string CollectionId { get; set; }
        public string ContentKey { get; set; }
        public string ContentKeyStartsWith { get; set; }
        public int? First { get; set; }
        public int? Offset { get; set; }
    }

    public class ContentItemQueryHandler : IQueryHandler<ContentItemQuery, ContentItem[]>
    {
        private readonly IContentStore _contentItemStore;

        public ContentItemQueryHandler(IContentStore contentItemStore)
        {
            _contentItemStore = contentItemStore;
        }

        public Task<ContentItem[]> Handle(ContentItemQuery query)
        {
            return _contentItemStore.GetContentItems(query);
        }
    }
}
