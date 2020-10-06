using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using System;
using GraphQLFieldType = GraphQL.Types.FieldType;

namespace AppText.Features.GraphQL.Types
{
    public static class ObjectGraphTypeExtensions
    {
        //public static void Field(
        //    this IObjectGraphType obj,
        //    string name,
        //    IGraphType type,
        //    string description = null,
        //    QueryArguments arguments = null,
        //    Func<IResolveFieldContext, object> resolve = null)
        //{
        //    var field = new GraphQLFieldType();
        //    field.Name = name;
        //    field.Description = description;
        //    field.Arguments = arguments;
        //    field.ResolvedType = type;
        //    field.Resolver = resolve != null ? new FuncFieldResolver<object>(resolve) : null;
        //    obj.AddField(field);
        //}
    }
}
