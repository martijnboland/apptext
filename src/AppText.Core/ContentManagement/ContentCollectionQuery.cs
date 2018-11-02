using AppText.Core.Shared.Queries;
using AppText.Core.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppText.Core.ContentManagement
{
    public class ContentCollectionQuery : IQuery<ContentCollection[]>
    {
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

        public ContentCollection[] Handle(ContentCollectionQuery query)
        {
            return _contentItemStore.GetContentCollections(query);
        }
    }
}
