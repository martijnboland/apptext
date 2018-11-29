using AppText.Core.Shared.Commands;
using AppText.Core.Storage;
using System.Threading.Tasks;

namespace AppText.Core.ContentManagement
{
    public class DeleteContentTypeCommand : ICommand
    {
        public string Id { get; }
        public string AppPublicId { get; }

        public DeleteContentTypeCommand(string appPublicId, string id)
        {
            this.Id = id;
            AppPublicId = appPublicId;
        }
    }

    public class DelecteContentTypeCommandHandler : ICommandHandler<DeleteContentTypeCommand>
    {
        private readonly IContentDefinitionStore _contentDefinitionStore;

        public DelecteContentTypeCommandHandler(IContentDefinitionStore contentDefinitionStore)
        {
            _contentDefinitionStore = contentDefinitionStore;
        }

        public async Task<CommandResult> Handle(DeleteContentTypeCommand command)
        {
            var result = new CommandResult();
            await _contentDefinitionStore.DeleteContentType(command.Id);
            return result;
        }
    }
}
