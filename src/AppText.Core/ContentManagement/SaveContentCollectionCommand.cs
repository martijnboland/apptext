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
        private readonly IContentStore _contentStore;
        private readonly ContentCollectionValidator _validator;

        public SaveContentCollectionCommandHandler(IContentStore contentStore, ContentCollectionValidator validator)
        {
            _contentStore = contentStore;
            _validator = validator;
        }

        public CommandResult Handle(SaveContentCollectionCommand command)
        {
            var result = new CommandResult();
            if (!_validator.IsValid(command.ContentCollection))
            {
                result.UpdateFromValidator(_validator);
            }
            else
            {
                if (command.ContentCollection.Id == null)
                {
                    _contentStore.AddContentCollection(command.ContentCollection);
                }
                else
                {
                    _contentStore.UpdateContentCollection(command.ContentCollection);
                }
                result.IsSuccess = true;
            }
            return result;
        }
    }
}
