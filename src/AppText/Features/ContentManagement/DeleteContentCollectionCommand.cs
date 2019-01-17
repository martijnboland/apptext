using AppText.Shared.Commands;
using AppText.Storage;
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
        private readonly IContentStore _contentItemStore;

        public DelecteContentCollectionCommandHandler(IContentStore contentItemStore)
        {
            _contentItemStore = contentItemStore;
        }

        public async Task<CommandResult> Handle(DeleteContentCollectionCommand command)
        {
            var result = new CommandResult();
            await _contentItemStore.DeleteContentCollection(command.Id, command.AppId);
            return result;
        }
    }
}
