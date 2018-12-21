using AppText.Core.Application;
using AppText.Core.ContentManagement;
using AppText.Core.Storage;
using GraphQL.Types;
using System;
using System.Threading.Tasks;

namespace AppText.Core.GraphQL.Types
{
    public class ContentItemType : ObjectGraphType<ContentItem>
    {
        private readonly ContentCollection _collection;
        private readonly Func<IContentStore> _getContentStore;

        public ContentItemType(ContentCollection contentCollection, Func<IContentStore> getContentStore)
        {
            _collection = contentCollection;
            _getContentStore = getContentStore;

            if (NameConverter.TryConvertToGraphQLName($"{contentCollection.Name}_{contentCollection.ContentType.Name}", out string graphQLName))
            {
                Name = graphQLName;

                Field(ci => ci.Id);
                Field(ci => ci.Version);
                Field(ci => ci.ContentKey);
                Field(ci => ci.CreatedAt, nullable: true);
                Field(ci => ci.CreatedBy, nullable: true);
                Field(ci => ci.LastModifiedAt, nullable: true);
                Field(ci => ci.LastModifiedBy, nullable: true);

                foreach (var metaField in contentCollection.ContentType.MetaFields)
                {
                    if (NameConverter.TryConvertToGraphQLName(metaField.Name, out string graphQLFieldName))
                    {
                        // TODO: resolve
                    }
                }
            }


        }
    }
}
