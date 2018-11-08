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

        public SaveContentTypeCommandHandler(IContentDefinitionStore store)
        {
            _store = store;
        }

        public CommandResult Handle(SaveContentTypeCommand command)
        {
            var result = new CommandResult();
            if (command.ContentType.Id == null)
            {
                _store.AddContentType(command.ContentType);
            }
            else
            {
                _store.UpdateContentType(command.ContentType);
            }
            return result;
        }
    }
}
