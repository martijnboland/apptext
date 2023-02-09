using AppText.Features.ContentManagement;
using AppText.Storage;
using GraphQL;
using GraphQL.Types;
using System;

namespace AppText.Features.GraphQL.Types
{
    public class ContentCollectionType : ObjectGraphType<ContentCollection>
    {
        public ContentCollectionType(ContentCollection contentCollection, Func<IContentStore> getContentStore, string[] languages, string defaultLanguage)
        {
            if (NameConverter.TryConvertToGraphQLName(contentCollection.Name, out string graphQLName))
            {
                Name = graphQLName;
                Description = "A collection that contains content of a specific type.";

                Field(cc => cc.Id).Description("The id of the content collection.");
                Field(cc => cc.Name).Description("The name of the content collection.");
                Field(cc => cc.Version).Description("The version of the content collection.");
                Field<ContentTypeType>("ContentType");

                var contentItemType = new ContentItemType(contentCollection, languages, defaultLanguage);
                var itemsType = new ListGraphType(contentItemType);
                this.Field("items", itemsType)
                    .Arguments(new QueryArguments(
                        new QueryArgument<StringGraphType>() { Name = "contentKeyStartsWith" },
                        new QueryArgument<IntGraphType>() { Name = "first" },
                        new QueryArgument<IntGraphType>() { Name = "offset" }
                    ))
                    .ResolveAsync(async ctx =>
                    {
                        var collection = ctx.Source;
                        if (collection != null)
                        {
                            var contentStore = getContentStore();
                            var contentItemQuery = new ContentItemQuery
                            {
                                AppId = contentCollection.AppId,
                                CollectionId = collection.Id,
                                ContentKeyStartsWith = ctx.GetArgument<string>("contentKeyStartsWith"),
                                First = ctx.GetArgument<int?>("first"),
                                Offset = ctx.GetArgument<int?>("offset")
                            };
                            return await contentStore.GetContentItems(contentItemQuery);
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
