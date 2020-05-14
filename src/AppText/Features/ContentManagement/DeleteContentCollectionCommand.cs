using AppText.Shared.Commands;
using AppText.Shared.Validation;
using AppText.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class DeleteContentCollectionCommand : ICommand
    {
        public string Id { get; }
        public string AppId { get; }

        public DeleteContentCollectionCommand(string id, string appId)
        {
            this.Id = id;
            this.AppId = appId;
        }
    }

    public class DelecteContentCollectionCommandHandler : ICommandHandler<DeleteContentCollectionCommand>
    {
        private readonly IContentStore _contentStore;

        public DelecteContentCollectionCommandHandler(IContentStore contentStore)
        {
            _contentStore = contentStore;
        }

        public async Task<CommandResult> Handle(DeleteContentCollectionCommand command)
        {
            var result = new CommandResult();
            // Verify that the collection has no content.
            if (await _contentStore.CollectionContainsContent(command.Id, command.AppId))
            {
                result.AddValidationError(new ValidationError() { Name = "", ErrorMessage = "AppText:DeleteCollectionContainsContent" });
            }
            if (! result.ValidationErrors.Any())
            {
                await _contentStore.DeleteContentCollection(command.Id, command.AppId);
            }
            return result;
        }
    }
}
