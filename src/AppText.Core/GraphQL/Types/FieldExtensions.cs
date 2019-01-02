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
                return localizableGraphType;
            }
            else
            {
                return scalarGraphType;
            }
        }
    }
}
