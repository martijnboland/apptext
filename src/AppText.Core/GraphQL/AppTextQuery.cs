using AppText.Core.Application;
using AppText.Core.ContentManagement;
using AppText.Core.GraphQL.Types;
using AppText.Core.Storage;
using GraphQL.Types;
using System;
using System.Threading.Tasks;

namespace AppText.Core.GraphQL
{
    public class AppTextQuery : ObjectGraphType
    {
        private readonly App _app;
        private readonly Func<IContentStore> _getContentStore;

        private AppTextQuery(App app, Func<IContentStore> getContentStore)
        {
            _app = app;
            _getContentStore = getContentStore;

            this.Name = app.Id;
            this.Description = $"GraphQL query schema for {app.DisplayName}";
        }

        public static Task<AppTextQuery> CreateAsync(App app, Func<IContentStore> getContentStore)
        {
            var query = new AppTextQuery(app, getContentStore);
            return query.InitializeFieldsAsync();
        }

        private async Task<AppTextQuery> InitializeFieldsAsync()
        {
            // Collections
            var contentStore = _getContentStore();
            var collections = await contentStore.GetContentCollections(new ContentCollectionQuery { AppId = _app.Id });

            foreach (var collection in collections)
            {
                if (NameConverter.TryConvertToGraphQLName(collection.Name, out string convertedName))
                {
                    var contentCollectionType = new ContentCollectionType(collection, _getContentStore, _app.Languages);
                    this.Field(convertedName, contentCollectionType, resolve: ctx => collection);
                }
                else
                {
                    // TODO: log why the collection is not added as a field
                }
            }

            // Languages
            this.Field("languages", new ListGraphType(new StringGraphType()), resolve: ctx => _app.Languages);
            this.Field("defaultLanguage", new StringGraphType(), resolve: ctx => _app.DefaultLanguage);

            return this;
        }
    }
}
