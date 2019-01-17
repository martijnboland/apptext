using AppText.Core.ContentDefinition;
using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AppText.Core.GraphQL.Types
{
    public static class FieldExtensions
    {
        public static IGraphType CreateGraphQLType(this Field field, string[] languages)
        {
            var scalarGraphType = field.FieldType.GraphQLType;

            if (field.FieldType.IsLocalizable)
            {
                var localizableGraphType = new ObjectGraphType
                {
                    Name = "LocalizableGraphType"
                };
                // Add a field for every language
                foreach (var language in languages)
                {
                    localizableGraphType.Field(language, scalarGraphType, resolve: ctx =>
                    {
                        var fieldValues = ctx.Source as JObject;
                        if (fieldValues != null && fieldValues.ContainsKey(language))
                        {
                            return fieldValues[language];
                        }
                        return null;
                    });
                }
                // Also add a language-neutral 'value' field
                localizableGraphType.Field("neutral", scalarGraphType, resolve: ctx =>
                {
                    var parsedScalarValue = scalarGraphType.ParseValue(ctx.Source);
                    if (parsedScalarValue == ctx.Source)
                    {
                        return parsedScalarValue;
                    }
                    return null;
                });
                return localizableGraphType;
            }
            else
            {
                return scalarGraphType;
            }
        }
    }
}
