using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using AppText.Shared.Commands;
using AppText.Shared.Validation;
using AppText.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class DeleteAppCommand : ICommand
    {
        public string Id { get; }

        public DeleteAppCommand(string id)
        {
            this.Id = id;
        }
    }

    public class DelecteAppCommandHandler : ICommandHandler<DeleteAppCommand>
    {
        private readonly IApplicationStore _applicationStore;
        private readonly IContentDefinitionStore _contentDefinitionStore;
        private readonly IContentStore _contentStore;

        public DelecteAppCommandHandler(IApplicationStore applicationStore, IContentDefinitionStore contentDefinitionStore, IContentStore contentStore)
        {
            _applicationStore = applicationStore;
            _contentDefinitionStore = contentDefinitionStore;
            _contentStore = contentStore;
        }

        public async Task<CommandResult> Handle(DeleteAppCommand command)
        {
            var result = new CommandResult();

            // Check if there are no collections for the given app
            var collections = await _contentStore.GetContentCollections(new ContentCollectionQuery { AppId = command.Id });
            if (collections.Any())
            {
                result.AddValidationError(new ValidationError { Name = "", ErrorMessage = "AppToDeleteHasCollections", Parameters = new[] { command.Id } });
            }
            else
            {
                // Delete content types and API keys
                var contentTypes = await _contentDefinitionStore.GetContentTypes(new ContentTypeQuery { AppId = command.Id });
                foreach (var contentType in contentTypes)
                {
                    await _contentDefinitionStore.DeleteContentType(contentType.Id, command.Id);
                }
                var apiKeys = await _applicationStore.GetApiKeys(new ApiKeysQuery { AppId = command.Id });
                foreach (var apiKey in apiKeys)
                {
                    await _applicationStore.DeleteApiKey(apiKey.Id, command.Id);
                }

                // Delete app
                await _applicationStore.DeleteApp(command.Id);
            }
            return result;
        }
    }
}
