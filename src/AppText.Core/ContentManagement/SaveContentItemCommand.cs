using AppText.Core.Shared.Commands;
using AppText.Core.Storage;

namespace AppText.Core.ContentManagement
{
    public class SaveContentItemCommand : ICommand
    {
        public SaveContentItemCommand(ContentItem contentItem)
        {
            this.ContentItem = contentItem;
        }

        public ContentItem ContentItem { get; }
    }

    public class SaveContentItemCommandHandler : ICommandHandler<SaveContentItemCommand>
    {
        private readonly IContentItemStore _store;

        public SaveContentItemCommandHandler(IContentItemStore store)
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
