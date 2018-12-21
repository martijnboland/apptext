using AppText.Core.ContentManagement;
using AppText.Core.GraphQL.Types;
using AppText.Core.Storage;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppText.Core.GraphQL
{
    public class SchemaResolver
    {
        private readonly IApplicationStore _applicationStore;
        private readonly Func<IContentStore> _getContentStore;
        private readonly ILogger<SchemaResolver> _logger;

        public SchemaResolver(ILogger<SchemaResolver> logger, IApplicationStore applicationStore, Func<IContentStore> getContentStore)
        {
            _applicationStore = applicationStore;
            _getContentStore = getContentStore;
            _logger = logger;
        }

        /// <summary>
        /// Gets the current GraphQL schema for the given appId. When it doesn't exist, a new schema is created based on the content collections.
        /// </summary>
        /// <returns></returns>
        public async Task<ISchema> Resolve(string appId)
        {
            var app = await _applicationStore.GetApp(appId);
            if (app == null)
            {
                throw new Exception($"Unable to generate GraphQL Schema for app {appId} because it was not found.");
            }

            var schema = new Schema();

            schema.Query = await AppTextQuery.CreateAsync(app, _getContentStore);

            return schema;

            //var person = new ObjectGraphType();
            //person.Name = "Person";
            //person.Field("name", new StringGraphType());
            //person.Field(
            //    "friends",
            //    new ListGraphType(new NonNullGraphType(person)),
            //    resolve: ctx => new[] { new SomeObject { Name = "Jaime" }, new SomeObject { Name = "Joe" } });

            //var root = new ObjectGraphType();
            //root.Name = "Root";
            //root.Field("hero", person, resolve: ctx => ctx.RootValue);
        }
    }

    public class SomeObject
    {
        public string Name { get; set; }
    }
}
