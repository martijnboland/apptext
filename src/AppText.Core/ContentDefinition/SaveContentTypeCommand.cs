using AppText.Core.Application;
using AppText.Core.Shared.Commands;
using AppText.Core.Shared.Validation;
using AppText.Core.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Core.ContentDefinition
{
    public class SaveContentTypeCommand : ICommand
    {
        public string AppPublicId { get; }
        public ContentType ContentType { get; }

        public SaveContentTypeCommand(string appPublicId, ContentType contentType)
        {
            this.ContentType = contentType;
            AppPublicId = appPublicId;
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

            if (! await _versioner.SetVersion(command.ContentType))
            {
                result.SetVersionError();
            }
            else
            {
                // Set app reference before validating
                var appReference = (await _applicationStore.GetApps(new AppQuery { PublicId = command.AppPublicId }))
                    .Select(a => new AppReference { Id = a.Id, PublicId = a.PublicId })
                    .FirstOrDefault();
                command.ContentType.App = appReference;
                
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
