using System.Threading.Tasks;

namespace AppText.Shared.Commands
{
    public interface IEventHandler<T> where T: IEvent
    {
        Task Handle(T publishedEvent);
    }
}
