using AppText.Shared.Commands;
using AppText.Storage;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class DeleteApiKeyCommand : ICommand
    {
        public string Id { get; }

        public string AppId { get; }

        public DeleteApiKeyCommand(string id, string appId)
        {
            Id = id;
            AppId = appId;
        }
    }

    public class DeleteApiKeyCommandHandler : ICommandHandler<DeleteApiKeyCommand>
    {
        private readonly IApplicationStore _applicationStore;

        public DeleteApiKeyCommandHandler(IApplicationStore applicationStore)
        {
            _applicationStore = applicationStore;
        }

        public async Task<CommandResult> Handle(DeleteApiKeyCommand command)
        {
            await _applicationStore.DeleteApiKey(command.Id, command.AppId);
            return new CommandResult();
        }
    }
}
