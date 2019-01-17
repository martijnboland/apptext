using AppText.Core.ContentManagement;
using AppText.Core.Storage;
using GraphQL.Types;
using System;

namespace AppText.Core.GraphQL.Types
{
    public class ContentCollectionType : ObjectGraphType<ContentCollection>
    {
        public ContentCollectionType(ContentCollection contentCollection, Func<IContentStore> getContentStore, string[] languages)
        {
            if (NameConverter.TryConvertToGraphQLName(contentCollection.Name, out string graphQLName))
            {
                Name = graphQLName;
                Description = "A collection that contains content of a specific type.";

                Field(cc => cc.Id).Description("The id of the content collection.");
                Field(cc => cc.Name).Description("The name of the content collection.");
                Field(cc => cc.Version).Description("The version of the content collection.");
                Field<ContentTypeType>("ContentType");

                var contentItemType = new ContentItemType(contentCollection, getContentStore, languages);
                var itemsType = new ListGraphType(contentItemType);
                this.Field("items", itemsType, resolve: ctx =>
                {
                    var collection = ctx.Source as ContentCollection;
                    if (collection != null)
                    {
                        var contentStore = getContentStore();
                        return contentStore.GetContentItems(new ContentItemQuery { AppId = contentCollection.ContentType.AppId, CollectionId = collection.Id });
                    }
                    else
                    {
                        return null;
                    }
                });
            }
            else
            {
                // TODO: log that a graphQL name could not be generated.
            }
        }
    }
}
