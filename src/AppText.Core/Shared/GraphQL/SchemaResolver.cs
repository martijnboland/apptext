using AppText.Core.Storage;
using GraphQL.Resolvers;
using GraphQL.Types;
using System;

namespace AppText.Core.Shared.GraphQL
{
    public class SchemaResolver
    {
        private readonly IContentStore _contentStore;

        public SchemaResolver(IContentStore contentStore)
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

    public static class ObjectGraphTypeExtensions
    {
        public static void Field(
            this IObjectGraphType obj,
            string name,
            IGraphType type,
            string description = null,
            QueryArguments arguments = null,
            Func<ResolveFieldContext, object> resolve = null)
        {
            var field = new FieldType();
            field.Name = name;
            field.Description = description;
            field.Arguments = arguments;
            field.ResolvedType = type;
            field.Resolver = resolve != null ? new FuncFieldResolver<object>(resolve) : null;
            obj.AddField(field);
        }
    }
}
