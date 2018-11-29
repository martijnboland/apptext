using AppText.Core.Shared.Commands;
using AppText.Core.Storage;
using System.Threading.Tasks;

namespace AppText.Core.ContentManagement
{
    public class DeleteContentItemCommand : ICommand
    {
        public string Id { get; }
        public string AppId { get; }

        public DeleteContentItemCommand(string appId, string id)
        {
            this.Id = id;
            this.AppId = appId;
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
            await _contentItemStore.DeleteContentItem(command.Id, command.AppId);
            return result;
        }
    }
}
