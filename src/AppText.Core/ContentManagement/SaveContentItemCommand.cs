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
        private readonly IVersioner _versioner;

        public SaveContentItemCommandHandler(IContentStore store, IVersioner versioner)
        {
            _store = store;
            _versioner = versioner;
        }

        public CommandResult Handle(SaveContentItemCommand command)
        {
            var result = new CommandResult();

            if (!_versioner.SetVersion(command.ContentItem))
            {
                result.SetVersionError();
            }
            else
            {
                if (command.ContentItem.Id == null)
                {
                    _store.AddContentItem(command.ContentItem);
                }
                else
                {
                    _store.UpdateContentItem(command.ContentItem);
                }
            }

            return result;
        }
    }
}
