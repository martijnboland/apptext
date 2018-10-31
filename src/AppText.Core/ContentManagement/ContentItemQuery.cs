using AppText.Core.Shared.Queries;
using AppText.Core.Storage;

namespace AppText.Core.ContentManagement
{
    public class ContentItemQuery : IQuery<ContentItem[]>
    {
        public string Id { get; set; }
        public string CollectionId { get; set; }
        public string Language { get; set; }
    }

    public class ContentItemQueryHandler : IQueryHandler<ContentItemQuery, ContentItem[]>
    {
        private readonly IContentItemStore _contentItemStore;

        public ContentItemQueryHandler(IContentItemStore contentItemStore)
        {
            _contentItemStore = contentItemStore;
        }

        public ContentItem[] Handle(ContentItemQuery query)
        {
            return _contentItemStore.GetContentItems(query);
        }
    }
}
