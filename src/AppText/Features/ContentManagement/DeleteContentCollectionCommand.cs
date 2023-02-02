using AppText.Shared.Commands;
using AppText.Shared.Infrastructure;
using AppText.Shared.Validation;
using AppText.Storage;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class DeleteContentCollectionCommand : ICommand
    {
        public string Id { get; }
        public string AppId { get; }
        public bool DeleteItems { get; }

        public DeleteContentCollectionCommand(string id, string appId, bool deleteItems = false)
        {
            this.Id = id;
            this.AppId = appId;
            this.DeleteItems = deleteItems;
        }
    }

    public class DelecteContentCollectionCommandHandler : ICommandHandler<DeleteContentCollectionCommand>
    {
        private readonly IContentStore _contentStore;
        private readonly IDispatcher _dispatcher;

        public DelecteContentCollectionCommandHandler(IContentStore contentStore, IDispatcher dispatcher)
        {
            _contentStore = contentStore;
            _dispatcher = dispatcher;
        }

        public async Task<CommandResult> Handle(DeleteContentCollectionCommand command)
        {
            var result = new CommandResult();
            // Verify that the collection has no content.
            if (await _contentStore.CollectionContainsContent(command.Id, command.AppId))
            {
                if (command.DeleteItems)
                {
                    await _contentStore.DeleteContentItemsForCollection(command.Id, command.AppId);
                }
                else
                {
                    result.AddValidationError(new ValidationError() { Name = "", ErrorMessage = "DeleteCollectionContainsContent" });
                }
            }
            if (! result.ValidationErrors.Any())
            {
                await _contentStore.DeleteContentCollection(command.Id, command.AppId);
                await _dispatcher.PublishEvent(new ContentCollectionDeletedEvent { AppId = command.AppId, CollectionId = command.Id });
            }
            return result;
        }
    }
}
