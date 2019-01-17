using System.Threading.Tasks;

namespace AppText.Shared.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task<CommandResult> Handle(T command);
    }
}