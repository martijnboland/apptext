using AppText.Core.Shared.Commands;
using AppText.Core.Storage;

namespace AppText.Core.ContentManagement
{
    public class DeleteContentItemCommand : ICommand
    {
        public string Id { get; }
        public string AppPublicId { get; }

        public DeleteContentItemCommand(string appPublicId, string id)
        {
            this.Id = id;
            AppPublicId = appPublicId;
        }
    }

    public class DelecteContentItemCommandHandler : ICommandHandler<DeleteContentItemCommand>
    {
        private readonly IContentStore _contentItemStore;

        public DelecteContentItemCommandHandler(IContentStore contentItemStore)
        {
            _contentItemStore = contentItemStore;
        }

        public CommandResult Handle(DeleteContentItemCommand command)
        {
            var result = new CommandResult();
            _contentItemStore.DeleteContentItem(command.Id);
            return result;
        }
    }
}
