using AppText.Core.GraphQL.Types;
using AppText.Core.Storage;
using GraphQL.Types;

namespace AppText.Core.GraphQL
{
    public class SchemaResolver
    {
        private readonly IContentStore _contentStore;

        public SchemaResolver(IContentStore contentStore, IContentDefinitionStore contentDefinitionStore)
        {
            _contentStore = contentStore;
        }

        /// <summary>
        /// Gets the current GraphQL schema for the given appId. When it doesn't exist, a new schema is created based on the content collections.
        /// </summary>
        /// <returns></returns>
        public ISchema Resolve(string appId)
        {
            var schema = new Schema();

            var person = new ObjectGraphType();
            person.Name = "Person";
            person.Field("name", new StringGraphType());
            person.Field(
                "friends",
                new ListGraphType(new NonNullGraphType(person)),
                resolve: ctx => new[] { new SomeObject { Name = "Jaime" }, new SomeObject { Name = "Joe" } });

            var root = new ObjectGraphType();
            root.Name = "Root";
            root.Field("hero", person, resolve: ctx => ctx.RootValue);

            schema.Query = root;
            schema.RegisterTypes(person);


            return schema;
        }
    }

    public class SomeObject
    {
        public string Name { get; set; }
    }
}
