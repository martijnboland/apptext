using AppText.Core.Shared.Commands;
using AppText.Core.Storage;
using System.Threading.Tasks;

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

        public async Task<CommandResult> Handle(DeleteContentItemCommand command)
        {
            var result = new CommandResult();
            await _contentItemStore.DeleteContentItem(command.Id);
            return result;
        }
    }
}
