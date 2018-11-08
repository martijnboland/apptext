using AppText.Core.Shared.Commands;
using AppText.Core.Storage;

namespace AppText.Core.ContentManagement
{
    public class DeleteContentTypeCommand : ICommand
    {
        public string Id { get; }

        public DeleteContentTypeCommand(string id)
        {
            this.Id = id;
        }
    }

    public class DelecteContentTypeCommandHandler : ICommandHandler<DeleteContentTypeCommand>
    {
        private readonly IContentDefinitionStore _contentDefinitionStore;

        public DelecteContentTypeCommandHandler(IContentDefinitionStore contentDefinitionStore)
        {
            _contentDefinitionStore = contentDefinitionStore;
        }

        public CommandResult Handle(DeleteContentTypeCommand command)
        {
            var result = new CommandResult();
            _contentDefinitionStore.DeleteContentType(command.Id);
            return result;
        }
    }
}
