using AppText.Core.Shared.Commands;
using AppText.Core.Storage;

namespace AppText.Core.ContentManagement
{
    public class SaveContentCollectionCommand : ICommand
    {
        public ContentCollection ContentCollection { get; }

        public SaveContentCollectionCommand(ContentCollection contentCollection)
        {
            this.ContentCollection = contentCollection;
        }
    }

    public class SaveContentCollectionCommandHandler : ICommandHandler<SaveContentCollectionCommand>
    {
        private readonly IContentStore _store;

        public SaveContentCollectionCommandHandler(IContentStore store)
        {
            _store = store;
        }

        public CommandResult Handle(SaveContentCollectionCommand command)
        {
            var result = new CommandResult();
            if (command.ContentCollection.Id == null)
            {
                _store.InsertContentCollection(command.ContentCollection);
            }
            else
            {
                _store.UpdateContentCollection(command.ContentCollection);
            }
            return result;
        }
    }
}
