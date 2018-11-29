using AppText.Core.Application;
using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Core.ContentManagement
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

        protected override async Task ValidateCustom(ContentItem objectToValidate)
        {
            // Verify app reference
            if (objectToValidate.App != null)
            {
                var app = (await _applicationStore.GetApps(new AppQuery { Id = objectToValidate.App.Id, PublicId = objectToValidate.App.PublicId })).SingleOrDefault();
                if (app == null)
                {
                    AddError(new ValidationError { Name = "App", ErrorMessage = "AppText:AppNotFound", Parameters = new[] { objectToValidate.App.Id } });
                }
            }

            // Validate content type based on content type in collection
            var collection = (await _contentStore.GetContentCollections(new ContentCollectionQuery { Id = objectToValidate.CollectionId })).FirstOrDefault();
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
            if (await _contentStore.ContentItemExists(objectToValidate.ContentKey, objectToValidate.CollectionId, objectToValidate.Id))
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
                else
                {
                    if (! field.FieldType.CanContainContent(contentPart.Value, contentMightBeLocalizable: true))
                    {
                        AddError(new ValidationError { Name = $"Content.{contentPart.Key}", ErrorMessage = "AppText:InvalidContentFieldValue", Parameters = new[] { contentPart.Value, field.FieldType.ToString() } });
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

            // Check if there are no missing required fields in the content item
            var metaFields = objectToValidate.Meta.Keys.ToArray();
            var missingMetaFields = contentType.MetaFields.Where(mf => mf.IsRequired && !metaFields.Contains(mf.Name, StringComparer.OrdinalIgnoreCase));
            AddErrors(missingMetaFields.Select(f => new ValidationError { Name = $"Meta", ErrorMessage = "AppText:MissingMetaFieldValue", Parameters = new[] { f.Name } } ) );

            var contentFields = objectToValidate.Content.Keys.ToArray();
            var missingContentFields = contentType.ContentFields.Where(cf => cf.IsRequired && !contentFields.Contains(cf.Name, StringComparer.OrdinalIgnoreCase));
            AddErrors(missingContentFields.Select(f => new ValidationError { Name = $"Content", ErrorMessage = "AppText:MissingContentFieldValue", Parameters = new[] { f.Name } } ) );
        }
    }
}
