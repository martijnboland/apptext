using AppText.Core.ContentDefinition;
using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Core.ContentManagement
{
    public class ContentCollectionValidator : Validator<ContentCollection>
    {
        private readonly IContentDefinitionStore _contentDefinitionStore;
        private readonly IContentStore _contentStore;
        private readonly IApplicationStore _applicationStore;

        public ContentCollectionValidator(IContentDefinitionStore contentDefinitionStore, IContentStore contentStore, IApplicationStore applicationStore)
        {
            _contentDefinitionStore = contentDefinitionStore;
            _contentStore = contentStore;
            _applicationStore = applicationStore;
        }

        protected override async Task ValidateCustom(ContentCollection objectToValidate)
        {
            // Check content type
            var contentTypeId = objectToValidate.ContentType.Id;
            var appId = objectToValidate.ContentType.AppId;
            var contentType = (await _contentDefinitionStore.GetContentTypes(new ContentTypeQuery { Id = contentTypeId, AppId = appId })).FirstOrDefault();
            if (contentType == null)
            {
                AddError("ContentType.Id", "AppText:UnknownContentType", contentTypeId);
            }
            else
            {
                // Sync content type with collection when valid
                objectToValidate.ContentType = contentType;

                if (objectToValidate.Id == null)
                {
                    // Check uniqueness of name
                    var otherCollection = (await _contentStore.GetContentCollections(new ContentCollectionQuery { Name = objectToValidate.Name, AppId = appId })).FirstOrDefault();
                    if (otherCollection != null)
                    {
                        AddError("Name", "AppText:DuplicateContentCollectionName", objectToValidate.Name);
                    }
                }
            }
        }
    }
}
