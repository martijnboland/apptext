using AppText.Shared.Commands;
using AppText.Storage;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class SaveContentCollectionCommand : ICommand
    {
        public ContentCollection ContentCollection { get; }
        public string AppId { get; }

        public SaveContentCollectionCommand(string appId, ContentCollection contentCollection)
        {
            this.ContentCollection = contentCollection;
            this.AppId = appId;
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

        public async Task<CommandResult> Handle(SaveContentCollectionCommand command)
        {
            var result = new CommandResult();
            command.ContentCollection.ContentType.AppId = command.AppId;

            if (! await _validator.IsValid(command.ContentCollection))
            {
                result.AddValidationErrors(_validator.Errors);
            }
            else
            {
                if (! await _versioner.SetVersion(command.AppId, command.ContentCollection))
                {
                    result.SetVersionError();
                }
                else
                {
                    if (command.ContentCollection.Id == null)
                    {
                        await _contentStore.AddContentCollection(command.ContentCollection);
                    }
                    else
                    {
                        await _contentStore.UpdateContentCollection(command.ContentCollection);
                    }
                }
            }
            return result;
        }
    }
}
