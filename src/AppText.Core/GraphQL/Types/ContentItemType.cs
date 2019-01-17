using AppText.Core.ContentDefinition;
using AppText.Core.ContentManagement;
using AppText.Core.Storage;
using GraphQL.Types;
using System;

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

                Field(ci => ci.Id).Description("Content item global unique identifier");
                Field(ci => ci.Version).Description("Content item version");
                Field(ci => ci.ContentKey).Description("Content item key. Use this key to retrieve individual content items.");
                Field<DateTimeGraphType>("CreatedAt");
                Field(ci => ci.CreatedBy, nullable: true).Description("The user that created the content item");
                Field<DateTimeGraphType>("LastModifiedAt");
                Field(ci => ci.LastModifiedBy, nullable: true).Description("The user that last modified the content item");

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
                    AddField(metaField, resolveFunc, "Meta field");
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
                    AddField(contentField, resolveFunc, "Content field");
                }
            }
            else
            {
                // TODO: log that a graphql name could not be generated
            }
        }

        private void AddField(Field field, Func<ResolveFieldContext, object> resolveFunc, string description = null)
        {
            if (NameConverter.TryConvertToGraphQLName(field.Name, out string graphQLFieldName))
            {
                // Create type from Field
                var fieldGraphType = field.CreateGraphQLType(_languages);
                this.Field(graphQLFieldName, fieldGraphType, resolve: resolveFunc, description: description);
            }
        }
    }
}
