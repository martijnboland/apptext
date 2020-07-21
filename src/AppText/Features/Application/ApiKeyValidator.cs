using AppText.Shared.Validation;
using AppText.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class ApiKeyValidator : Validator<ApiKey>
    {
        private readonly IApplicationStore _applicationStore;

        public ApiKeyValidator(IApplicationStore applicationStore)
        {
            _applicationStore = applicationStore;
        }

        protected override async Task ValidateCustom(ApiKey objectToValidate)
        {
            // Check duplicate key name
            var existingApiKey = (await _applicationStore.GetApiKeys(new ApiKeysQuery { AppId = objectToValidate.AppId, Name = objectToValidate.Name })).FirstOrDefault();
            if (existingApiKey != null)
            {
                AddError(new ValidationError { Name = "Name", ErrorMessage = "DuplicateApiKeyName", Parameters = new[] { objectToValidate.Name } });
            }
        }
    }
}
