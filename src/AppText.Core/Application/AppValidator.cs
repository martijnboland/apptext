using AppText.Core.Shared.Validation;
using AppText.Core.Storage;

namespace AppText.Core.Application
{
    public class AppValidator : Validator<App>
    {
        private readonly IApplicationStore _store;

        public AppValidator(IApplicationStore store)
        {
            _store = store;
        }

        protected override void ValidateCustom(App objectToValidate)
        {
            // Check uniqueness public identifier
            if (_store.AppExists(objectToValidate.PublicId, objectToValidate.Id))
            {
                AddError(new ValidationError
                {
                    Name = "PublicId",
                    ErrorMessage = "AppText:PublicIdAlreadyExists",
                    Parameters = new[] { objectToValidate.PublicId }
                });
            }
        }
    }
}
