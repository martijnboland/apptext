using AppText.Shared.Commands;
using AppText.Shared.Queries;
using System;
using System.Threading.Tasks;

namespace AppText.Shared.Infrastructure
{
    /// <summary>
    /// Mediator class for dispatching queries and commands.
    /// </summary>
    public class Dispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<CommandResult> ExecuteCommand<T>(T command) where T: ICommand
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = _serviceProvider.GetService(handlerType) as ICommandHandler<T>;
            if (handler != null)
            {
                return handler.Handle(command);
            }
            throw new Exception("No handler found for command {0} " + command.ToString());
        }

        public Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            var queryType = query.GetType();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));
            // Cast the resolved handler to dynamic because we can't cast the resolved object to IQueryHandler<IQuery<TResult>, TResult>>
            // See also https://cuttingedge.it/blogs/steven/pivot/entry.php?id=92.
            var handler = (dynamic)_serviceProvider.GetService(handlerType);
            if (handler != null)
            {
                return handler.Handle((dynamic)query);
            }
            throw new Exception("No handler found for command {0} " + query.ToString());
        }
    }
}
