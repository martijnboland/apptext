using AppText.Core.Shared.Queries;
using AppText.Core.Storage;
using System.Threading.Tasks;

namespace AppText.Core.ContentManagement
{
    public class ContentItemQuery : IQuery<ContentItem[]>
    {
        public string AppPublicId { get; set; }
        public string Id { get; set; }
        public string CollectionId { get; set; }
        public string Language { get; set; }
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
