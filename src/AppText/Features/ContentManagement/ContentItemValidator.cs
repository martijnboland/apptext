using AppText.Shared.Validation;
using AppText.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class ContentItemValidator : Validator<ContentItem>
    {
        private readonly IContentStore _contentStore;
        private readonly IApplicationStore _applicationStore;

        public ContentItemValidator(IContentStore contentStore, IApplicationStore applicationStore)
        {
            _contentStore = contentStore;
            _applicationStore = applicationStore;
        }

        public async Task<bool> IsValidForLanguages(ContentItem objectToValidate, string[] languagesToValidate)
        {
            ClearErrors();
            await ValidateAsync(objectToValidate);
            await ValidateForLanguages(objectToValidate, languagesToValidate);

            return Errors.Count() == 0;
        }

        public async Task ValidateForLanguages(ContentItem objectToValidate, string[] languagesToValidate)
        {
            // Verify app reference
            if (!String.IsNullOrEmpty(objectToValidate.AppId))
            {
                var app = await _applicationStore.GetApp(objectToValidate.AppId);
                if (app == null)
                {
                    AddError(new ValidationError { Name = "App", ErrorMessage = "AppText:AppNotFound", Parameters = new[] { objectToValidate.AppId } });
                    return;
                }
            }

            // Validate content type based on content type in collection
            var collection = (await _contentStore.GetContentCollections(new ContentCollectionQuery { Id = objectToValidate.CollectionId, AppId = objectToValidate.AppId })).FirstOrDefault();
            if (collection == null)
            {
                AddError(new ValidationError
                {
                    Name = "CollectionId",
                    ErrorMessage = "AppText:UnknownCollection",
                    Parameters = new[] { objectToValidate.CollectionId }
                });
                return;
            }

            // Check uniqueness of key
            if (await _contentStore.ContentItemExists(objectToValidate.ContentKey, objectToValidate.CollectionId, objectToValidate.Id, objectToValidate.AppId))
            {
                AddError(new ValidationError
                {
                    Name = "ContentKey",
                    ErrorMessage = "AppText:ContentKeyAlreadyExists",
                    Parameters = new[] { objectToValidate.ContentKey, collection.Name, objectToValidate.Id }
                });
                return;
            }

            var contentType = collection.ContentType;

            // Check if content fields are in content type and have the correct type
            foreach (var contentPart in objectToValidate.Content)
            {
                var field = contentType.ContentFields.FirstOrDefault(cf => String.Compare(cf.Name, contentPart.Key, StringComparison.OrdinalIgnoreCase) == 0);
                if (field == null)
                {
                    AddError(new ValidationError { Name = "Content", ErrorMessage = "AppText:UnknownContentField", Parameters = new[] { contentPart.Key } });
                }
            }

            // Check if content values are compatible with the fields in the content type
            foreach(var contentField in contentType.ContentFields)
            {
                var contentObj = objectToValidate.Content.ContainsKey(contentField.Name) ? objectToValidate.Content[contentField.Name] : null;
                if (contentField.IsLocalizable)
                {
                    // Localizable object. This must be a JObject. If so, validate the properties by language.
                    var contentJObject = contentObj as JObject;
                    // Validate content values for each language
                    foreach (var language in languagesToValidate)
                    {
                        if (contentField.IsRequired && (contentJObject == null || ! contentJObject.ContainsKey(language) || contentJObject[language] == null))
                        {
                            AddError(new ValidationError { Name = $"Content.{contentField.Name}.{language}", ErrorMessage = "AppText:MissingContentFieldValue", Parameters = new[] { contentField.Name, language } });
                        }
                        else if (contentJObject != null && contentJObject.ContainsKey(language))
                        {
                            var localizedValue = contentJObject[language].ToObject<object>();
                            if (!contentField.FieldType.CanContainContent(localizedValue))
                            {
                                AddError(new ValidationError { Name = $"Content.{contentField.Name}.{language}", ErrorMessage = "AppText:InvalidContentFieldValue", Parameters = new[] { localizedValue, contentField.FieldType.ToString(), language } });
                            }
                        }
                    }
                }
                else
                {
                    if (contentObj == null && contentField.IsRequired)
                    {
                        AddError(new ValidationError { Name = $"Content.{contentField.Name}", ErrorMessage = "AppText:MissingContentFieldValue", Parameters = new[] { contentField.Name } });
                    }
                    else if (!contentField.FieldType.CanContainContent(contentObj))
                    {
                        AddError(new ValidationError { Name = $"Content.{contentField.Name}", ErrorMessage = "AppText:InvalidContentFieldValue", Parameters = new[] { contentObj, contentField.FieldType.ToString() } });
                    }
                }
            }

            // Check meta fields
            foreach (var metaPart in objectToValidate.Meta)
            {
                var field = contentType.MetaFields.FirstOrDefault(cf => String.Compare(cf.Name, metaPart.Key, StringComparison.OrdinalIgnoreCase) == 0);
                if (field == null)
                {
                    AddError(new ValidationError { Name = "Meta", ErrorMessage = "AppText:UnknownMetaField", Parameters = new[] { metaPart.Key } });
                }
                else
                {
                    if (!field.FieldType.CanContainContent(metaPart.Value))
                    {
                        AddError(new ValidationError { Name = $"Meta.{metaPart.Key}", ErrorMessage = "AppText:InvalidMetaFieldValue", Parameters = new[] { metaPart.Value, field.FieldType.ToString() } });
                    }
                }
            }

            // Check if there are no missing required meta fields in the content item
            var metaFields = objectToValidate.Meta.Keys.ToArray();
            var missingMetaFields = contentType.MetaFields.Where(mf => mf.IsRequired && !metaFields.Contains(mf.Name, StringComparer.OrdinalIgnoreCase));
            AddErrors(missingMetaFields.Select(f => new ValidationError { Name = $"Meta.{f.Name}", ErrorMessage = "AppText:MissingMetaFieldValue", Parameters = new[] { f.Name } }));
        }
    }
}
