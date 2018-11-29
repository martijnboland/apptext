using AppText.Core.Shared.Commands;
using AppText.Core.Storage;
using System.Threading.Tasks;

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

        public async Task<CommandResult> Handle(DeleteAppCommand command)
        {
            var result = new CommandResult();
            // TODO: check existence of content types, collections and content items
            await _store.DeleteApp(command.Id);
            return result;
        }
    }
}
