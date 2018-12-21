using AppText.Core.ContentManagement;
using AppText.Core.Storage;
using GraphQL.Types;
using System;

namespace AppText.Core.GraphQL.Types
{
    public class ContentCollectionType : ObjectGraphType<ContentCollection>
    {
        public ContentCollectionType(ContentCollection contentCollection, Func<IContentStore> getContentStore)
        {
            Name = "ContentCollection";
            Description = "A collection that contains content of a specific type.";

            Field(d => d.Id).Description("The id of the content collection.");
            Field(d => d.Name).Description("The name of the content collection.");
            Field(d => d.Version).Description("The version of the content collection.");

            var contentItemType = new ContentItemType(contentCollection, getContentStore);
            var itemsType = new ListGraphType(contentItemType);
            this.Field("items", itemsType, resolve: ctx =>
            {
                var collection = ctx.Source as ContentCollection;
                if (collection != null)
                {
                    var contentStore = getContentStore();
                    return contentStore.GetContentItems(new ContentItemQuery { CollectionId = collection.Id });
                }
                else
                {
                    return null;
                }
            });
        }
    }
}
