using AppText.Core.ContentDefinition;
using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using System.Linq;

namespace AppText.Core.ContentManagement
{
    public class ContentCollectionValidator : Validator<ContentCollection>
    {
        private readonly IContentDefinitionStore _contentDefinitionStore;
        private readonly IContentStore _contentStore;

        public ContentCollectionValidator(IContentDefinitionStore contentDefinitionStore, IContentStore contentStore)
        {
            _contentDefinitionStore = contentDefinitionStore;
            _contentStore = contentStore;
        }

        protected override void ValidateCustom(ContentCollection contentCollection)
        {
            // Check content type
            var contentTypeId = contentCollection.ContentType.Id;
            var contentType = _contentDefinitionStore.GetContentTypes(new ContentTypeQuery { Id = contentTypeId }).FirstOrDefault();
            if (contentType == null)
            {
                AddError("ContentType.Id", "AppText:UnknownContentType", contentTypeId);
            }
            else
            {
                // Sync content type with collection when valid
                contentCollection.ContentType = contentType;

                if (contentCollection.Id == null)
                {
                    // Check uniqueness of name
                    var otherCollection = _contentStore.GetContentCollections(new ContentCollectionQuery { Name = contentCollection.Name }).FirstOrDefault();
                    if (otherCollection != null)
                    {
                        AddError("Name", "AppText:DuplicateContentCollectionName", contentCollection.Name);
                    }
                }
            }
        }
    }
}
