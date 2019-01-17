using AppText.Shared.Commands;
using AppText.Storage;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class DeleteContentTypeCommand : ICommand
    {
        public string Id { get; }
        public string AppId { get; }

        public DeleteContentTypeCommand(string appId, string id)
        {
            this.Id = id;
            this.AppId = appId;
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
            await _contentDefinitionStore.DeleteContentType(command.Id, command.AppId);
            return result;
        }
    }
}
