using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using System.Threading.Tasks;

namespace AppText.Core.Application
{
    public class AppValidator : Validator<App>
    {
        private readonly IApplicationStore _store;

        public AppValidator(IApplicationStore store)
        {
            _store = store;
        }

        protected override async Task ValidateCustom(App objectToValidate)
        {
            // Check uniqueness public identifier
            if (await _store.AppExists(objectToValidate.PublicId, objectToValidate.Id))
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
