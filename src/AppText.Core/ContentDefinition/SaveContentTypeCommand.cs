using AppText.Core.Shared.Commands;
using AppText.Core.Storage;
using System.Threading.Tasks;

namespace AppText.Core.ContentDefinition
{
    public class SaveContentTypeCommand : ICommand
    {
        public string AppId { get; }
        public ContentType ContentType { get; }

        public SaveContentTypeCommand(string appId, ContentType contentType)
        {
            this.ContentType = contentType;
            this.AppId = appId;
        }
    }

    public class SaveContentTypeCommandHandler : ICommandHandler<SaveContentTypeCommand>
    {
        private IContentDefinitionStore _store;
        private readonly IVersioner _versioner;
        private readonly ContentTypeValidator _validator;
        private readonly IApplicationStore _applicationStore;

        public SaveContentTypeCommandHandler(IContentDefinitionStore store, IApplicationStore applicationStore, IVersioner versioner, ContentTypeValidator validator)
        {
            _store = store;
            _versioner = versioner;
            _validator = validator;
            _applicationStore = applicationStore;
        }

        public async Task<CommandResult> Handle(SaveContentTypeCommand command)
        {
            var result = new CommandResult();

            if (! await _versioner.SetVersion(command.AppId, command.ContentType))
            {
                result.SetVersionError();
            }
            else
            {
                if (! await _validator.IsValid(command.ContentType))
                {
                    result.AddValidationErrors(_validator.Errors);
                }
                else
                {
                    if (command.ContentType.Id == null)
                    {
                        await _store.AddContentType(command.ContentType);
                    }
                    else
                    {
                        await _store.UpdateContentType(command.ContentType);
                    }
                }
            }
            return result;
        }
    }
}
