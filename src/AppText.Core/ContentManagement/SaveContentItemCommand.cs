using AppText.Core.Shared.Commands;
using AppText.Core.Storage;

namespace AppText.Core.ContentManagement
{
    public class SaveContentItemCommand : ICommand
    {
        public ContentItem ContentItem { get; }

        public SaveContentItemCommand(ContentItem contentItem)
        {
            this.ContentItem = contentItem;
        }
    }

    public class SaveContentItemCommandHandler : ICommandHandler<SaveContentItemCommand>
    {
        private readonly IContentStore _store;

        public SaveContentItemCommandHandler(IContentStore store)
        {
            _store = store;
        }

        public CommandResult Handle(SaveContentItemCommand command)
        {
            var result = new CommandResult();
            if (command.ContentItem.Id == null)
            {
                _store.InsertContentItem(command.ContentItem);
            }
            else
            {
                _store.UpdateContentItem(command.ContentItem);
            }
            return result;
        }
    }
}
