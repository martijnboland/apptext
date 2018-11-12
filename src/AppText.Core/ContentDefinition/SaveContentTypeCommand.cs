using AppText.Core.Shared.Commands;
using AppText.Core.Storage;

namespace AppText.Core.ContentDefinition
{
    public class SaveContentTypeCommand : ICommand
    {
        public ContentType ContentType { get; }

        public SaveContentTypeCommand(ContentType contentType)
        {
            this.ContentType = contentType;
        }
    }

    public class SaveContentTypeCommandHandler : ICommandHandler<SaveContentTypeCommand>
    {
        private IContentDefinitionStore _store;
        private readonly IVersioner _versioner;

        public SaveContentTypeCommandHandler(IContentDefinitionStore store, IVersioner versioner)
        {
            _store = store;
            _versioner = versioner;
        }

        public CommandResult Handle(SaveContentTypeCommand command)
        {
            var result = new CommandResult();

            if (! _versioner.SetVersion(command.ContentType))
            {
                result.SetVersionError();
            }
            else
            {
                if (command.ContentType.Id == null)
                {
                    _store.AddContentType(command.ContentType);
                }
                else
                {
                    _store.UpdateContentType(command.ContentType);
                }
            }
            return result;
        }
    }
}
