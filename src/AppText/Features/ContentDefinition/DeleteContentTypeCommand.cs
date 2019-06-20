using AppText.Features.ContentDefinition;
using AppText.Shared.Commands;
using AppText.Shared.Validation;
using AppText.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class DeleteContentTypeCommand : ICommand
    {
        public string Id { get; }
        public string AppId { get; }

        public DeleteContentTypeCommand(string appId, string id)
        {
            this.Id = id;
            this.AppId = appId;
        }
    }

    public class DelecteContentTypeCommandHandler : ICommandHandler<DeleteContentTypeCommand>
    {
        private readonly IContentDefinitionStore _contentDefinitionStore;
        private readonly IContentStore _contentStore;

        public DelecteContentTypeCommandHandler(IContentDefinitionStore contentDefinitionStore, IContentStore contentStore)
        {
            _contentDefinitionStore = contentDefinitionStore;
            _contentStore = contentStore;
        }

        public async Task<CommandResult> Handle(DeleteContentTypeCommand command)
        {
            var result = new CommandResult();
            // Verify that no collection with the content type exists (except snapshot collections).
            var collections = await _contentStore.GetContentCollections(new ContentCollectionQuery { AppId = command.AppId });
            // TODO: exclude snapshot collections
            if (collections.Any(c => c.ContentType.Id == command.Id))
            {
                var collectionNames = collections.Where(c => c.ContentType.Id == command.Id).Select(c => c.Name);
                result.AddValidationError(new ValidationError() { Name = "", ErrorMessage = "AppText:DeleteContentTypeInUse", Parameters = new[] { string.Join(",", collectionNames) } });
            }

            if (!result.ValidationErrors.Any())
            {
                await _contentDefinitionStore.DeleteContentType(command.Id, command.AppId);
            }
            return result;
        }
    }
}
