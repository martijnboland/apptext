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
        private readonly IVersioner _versioner;

        public SaveContentCollectionCommandHandler(IContentStore contentStore, ContentCollectionValidator validator, IVersioner versioner)
        {
            _contentStore = contentStore;
            _validator = validator;
            _versioner = versioner;
        }

        public CommandResult Handle(SaveContentCollectionCommand command)
        {
            var result = new CommandResult();

            if (!_validator.IsValid(command.ContentCollection))
            {
                result.AddValidationErrors(_validator.Errors);
            }
            else
            {
                if (!_versioner.SetVersion(command.ContentCollection))
                {
                    result.SetVersionError();
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
                }
            }
            return result;
        }
    }
}
