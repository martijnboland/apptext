using AppText.Shared.Commands;
using AppText.Storage;
using System;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class CreateApiKeyCommand : ICommand
    {
        public string AppId { get; set; }

        public string Name { get; set; }
    }

    public class CreateApiKeyCommandHandler : ICommandHandler<CreateApiKeyCommand>
    {
        private readonly ApiKeyValidator _validator;
        private readonly IApplicationStore _applicationStore;

        public CreateApiKeyCommandHandler(ApiKeyValidator validator, IApplicationStore applicationStore)
        {
            _validator = validator;
            _applicationStore = applicationStore;
        }

        public async Task<CommandResult> Handle(CreateApiKeyCommand command)
        {
            var result = new CommandResult();

            var readableKey = Guid.NewGuid().ToString("N");
            var apiKey = new ApiKey
            {
                AppId = command.AppId,
                Name = command.Name,
                CreatedAt = DateTimeOffset.UtcNow,
                Key = ApiKey.HashKey(readableKey, command.AppId)
            };

            if (!await _validator.IsValid(apiKey))
            {
                result.AddValidationErrors(_validator.Errors);
            }
            else
            {
                await _applicationStore.AddApiKey(apiKey);
                var resultData = new CreateApiKeyResultData
                {
                    ReadableKey = readableKey,
                    ApiKey = apiKey
                };
                result.SetResultData(resultData);
            }
            return result;
        }
    }

    public class CreateApiKeyResultData
    {
        public string ReadableKey { get; set; }
        public ApiKey ApiKey { get; set; }
    }
}
