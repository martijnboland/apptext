using AppText.Core.Application;
using AppText.Core.ContentDefinition;
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
        private readonly string[] _languages;

        public ContentItemType(ContentCollection contentCollection, Func<IContentStore> getContentStore, string[] languages)
        {
            _collection = contentCollection;
            _getContentStore = getContentStore;
            _languages = languages;

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
                    Func<ResolveFieldContext, object> resolveFunc = ctx =>
                    {
                        var contentItem = ctx.Source as ContentItem;
                        if (contentItem != null)
                        {
                            return contentItem.Meta[metaField.Name];
                        }
                        else
                        {
                            return null;
                        }
                    };
                    AddField(metaField, resolveFunc);
                }

                foreach (var contentField in contentCollection.ContentType.ContentFields)
                {
                    Func<ResolveFieldContext, object> resolveFunc = ctx =>
                    {
                        var contentItem = ctx.Source as ContentItem;
                        if (contentItem != null)
                        {
                            return contentItem.Content[contentField.Name];
                        }
                        else
                        {
                            return null;
                        }
                    };
                    AddField(contentField, resolveFunc);
                }
            }
            else
            {
                // TODO: log that a graphql name could not be generated
            }
        }

        private void AddField(Field field, Func<ResolveFieldContext, object> resolveFunc)
        {
            if (NameConverter.TryConvertToGraphQLName(field.Name, out string graphQLFieldName))
            {
                // Create type from Field
                var fieldGraphType = field.CreateGraphQLType(_languages);
                this.Field(graphQLFieldName, fieldGraphType, resolve: resolveFunc);
            }
        }
    }
}
