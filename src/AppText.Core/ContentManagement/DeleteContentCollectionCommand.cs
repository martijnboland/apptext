using AppText.Core.Shared.Commands;
using AppText.Core.Storage;

namespace AppText.Core.ContentManagement
{
    public class DeleteContentCollectionCommand : ICommand
    {
        public string Id { get; }

        public DeleteContentCollectionCommand(string id)
        {
            this.Id = id;
        }
    }

    public class DelecteContentCollectionCommandHandler : ICommandHandler<DeleteContentCollectionCommand>
    {
        private readonly IContentStore _contentItemStore;

        public DelecteContentCollectionCommandHandler(IContentStore contentItemStore)
        {
            _contentItemStore = contentItemStore;
        }

        public CommandResult Handle(DeleteContentCollectionCommand command)
        {
            var result = new CommandResult();
            _contentItemStore.DeleteContentCollection(command.Id);
            return result;
        }
    }
}
