using AppText.Core.Shared.Commands;
using AppText.Core.Storage;

namespace AppText.Core.Application
{
    public class DeleteAppCommand : ICommand
    {
        public string Id { get; }

        public DeleteAppCommand(string id)
        {
            this.Id = id;
        }
    }

    public class DelecteAppCommandHandler : ICommandHandler<DeleteAppCommand>
    {
        private readonly IApplicationStore _store;

        public DelecteAppCommandHandler(IApplicationStore store)
        {
            _store = store;
        }

        public CommandResult Handle(DeleteAppCommand command)
        {
            var result = new CommandResult();
            // TODO: check existence of content types, collections and content items
            _store.DeleteApp(command.Id);
            return result;
        }
    }
}
