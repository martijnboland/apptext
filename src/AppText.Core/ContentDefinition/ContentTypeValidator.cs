using AppText.Core.Application;
using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using System.Linq;

namespace AppText.Core.ContentDefinition
{
    public class ContentTypeValidator : Validator<ContentType>
    {
        private readonly IApplicationStore _applicationStore;

        public ContentTypeValidator(IApplicationStore applicationStore)
        {
            _applicationStore = applicationStore;
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
        }
    }
}
