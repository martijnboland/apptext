using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppText.Features.GraphQL.Types
{
    public class ContentItemType : ObjectGraphType<ContentItem>
    {
        private readonly string[] _languages;
        private readonly string _defaultLanguage;

        public ContentItemType(ContentCollection contentCollection, string[] languages, string defaultLanguage)
        {
            _languages = languages;
            _defaultLanguage = defaultLanguage;

            if (NameConverter.TryConvertToGraphQLName($"{contentCollection.Name}_{contentCollection.ContentType.Name}", out string graphQLName))
            {
                Name = graphQLName;

                Field(ci => ci.Id).Description("Content item global unique identifier");
                Field(ci => ci.Version).Description("Content item version");
                Field(ci => ci.ContentKey).Description("Content item key. Use this key to retrieve individual content items.");
                Field<DateTimeGraphType>("CreatedAt");
                Field(ci => ci.CreatedBy, nullable: true).Description("The user that created the content item");
                Field<DateTimeGraphType>("LastModifiedAt");
                Field(ci => ci.LastModifiedBy, nullable: true).Description("The user that last modified the content item");

                foreach (var metaField in contentCollection.ContentType.MetaFields)
                {
                    Func<IResolveFieldContext, object> resolveFunc = ctx =>
                    {
                        var contentItem = ctx.Source as ContentItem;
                        if (contentItem != null)
                        {
                            return contentItem.Meta[metaField.Name];
                        }
                        else
                        {
                            return null;
                        }
                    };
                    AddField(metaField, resolveFunc, "Meta field");
                }

                foreach (var contentField in contentCollection.ContentType.ContentFields)
                {
                    Func<IResolveFieldContext, object> resolveFunc = ctx =>
                    {
                        var contentItem = ctx.Source as ContentItem;
                        if (contentItem != null)
                        {
                            if (contentField.IsLocalizable)
                            {
                                // Localizable field, check if a valid language argument is given and return the value for the requested language.
                                // Otherwise, return the value for the default language of the app.
                                var language = _defaultLanguage;
                                if (ctx.HasArgument("language"))
                                {
                                    language = ctx.Arguments["language"].Value?.ToString();
                                    if (!_languages.Any(l => l == language))
                                    {
                                        ctx.Errors.Add(new ExecutionError($"The language argument '{language}' for the '{contentField.Name}' field is not allowed for this app."));
                                    }
                                }

                                if (contentItem.Content.ContainsKey(contentField.Name))
                                {
                                    var field = contentItem.Content[contentField.Name];
                                    Dictionary<string, object> fieldValues = null;

                                    // Source can be a JObject or Dictionary
                                    if (field is JObject)
                                    {
                                        fieldValues = JObject.FromObject(field).ToObject<Dictionary<string, object>>();
                                    }
                                    else
                                    {
                                        fieldValues = field as Dictionary<string, object>;
                                    }

                                    if (fieldValues != null && fieldValues.ContainsKey(language))
                                    {
                                        return fieldValues[language];
                                    }
                                }
                                return null;
                            }
                            else
                            {
                                // Non-localizable field, return value directly
                                return contentItem.Content[contentField.Name];
                            }
                        }
                        else
                        {
                            return null;
                        }
                    };
                    AddField(contentField, resolveFunc, "Content field");
                }
            }
            else
            {
                // TODO: log that a graphql name could not be generated
            }
        }

        private void AddField(Field field, Func<IResolveFieldContext, object> resolveFunc, string description = null)
        {
            if (NameConverter.TryConvertToGraphQLName(field.Name, out string graphQLFieldName))
            {
                // Create type from Field
                var arguments = new QueryArguments();
                if (field.IsLocalizable)
                {
                    arguments.Add(new QueryArgument<StringGraphType>() { Name = "language" });
                }
                this.Field(graphQLFieldName, field.FieldType.GraphQLType)
                    .Resolve(resolveFunc)
                    .Description(description)
                    .Arguments(arguments);
            }
        }
    }
}
