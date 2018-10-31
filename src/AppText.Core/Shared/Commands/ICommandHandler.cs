namespace AppText.Core.Shared.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        CommandResult Handle(T command);
    }
}