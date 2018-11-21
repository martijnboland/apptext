using AppText.Core.Application;
using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using System.Linq;

namespace AppText.Core.ContentDefinition
{
    public class ContentTypeValidator : Validator<ContentType>
    {
        private readonly IApplicationStore _applicationStore;
        private readonly IContentDefinitionStore _contentDefinitionStore;

        public ContentTypeValidator(IApplicationStore applicationStore, IContentDefinitionStore contentDefinitionStore)
        {
            _applicationStore = applicationStore;
            _contentDefinitionStore = contentDefinitionStore;
        }

        protected override void ValidateCustom(ContentType objectToValidate)
        {
            // Verify app reference
            if (objectToValidate.App != null)
            {
                var app = _applicationStore.GetApps(new AppQuery { Id = objectToValidate.App.Id, PublicId = objectToValidate.App.PublicId }).SingleOrDefault();
                if (app == null)
                {
                    AddError(new ValidationError { Name = "App", ErrorMessage = "AppText:AppNotFound", Parameters = new[] { objectToValidate.App.Id } } );
                }
            }

            // Duplicate content type name
            if (objectToValidate.Id == null)
            {
                if (_contentDefinitionStore.GetContentTypes(new ContentTypeQuery { AppPublicId = objectToValidate.App.PublicId, Name = objectToValidate.Name }).Length > 0)
                {
                    AddError(new ValidationError { Name = "Name", ErrorMessage = "AppText:DuplicateContentTypeName", Parameters = new[] { objectToValidate.Name } });
                }
            }
        }
    }
}
