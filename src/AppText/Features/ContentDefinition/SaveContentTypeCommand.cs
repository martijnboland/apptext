using AppText.Shared.Commands;
using AppText.Shared.Infrastructure;
using AppText.Storage;
using System.Threading.Tasks;

namespace AppText.Features.ContentDefinition
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
        private readonly IDispatcher _dispatcher;

        public SaveContentTypeCommandHandler(IContentDefinitionStore store, IVersioner versioner, ContentTypeValidator validator, IDispatcher dispatcher)
        {
            _store = store;
            _versioner = versioner;
            _validator = validator;
            _dispatcher = dispatcher;
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
                        await _dispatcher.PublishEvent(new ContentTypeChangedEvent { ContentType = command.ContentType });
                    }
                }
            }
            return result;
        }
    }
}
