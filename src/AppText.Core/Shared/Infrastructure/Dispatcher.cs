using AppText.Core.Shared.Commands;
using AppText.Core.Shared.Queries;
using System;

namespace AppText.Core.Infrastructure
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

        public CommandResult ExecuteCommand<T>(T command) where T: ICommand
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = _serviceProvider.GetService(handlerType) as ICommandHandler<T>;
            if (handler != null)
            {
                return handler.Handle(command);
            }
            throw new Exception("No handler found for command {0} " + command.ToString());
        }

        public TResult ExecuteQuery<TQuery, TResult>(TQuery query) where TQuery: IQuery<TResult>
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) as IQueryHandler<TQuery, TResult>;
            if (handler != null)
            {
                return handler.Handle(query);
            }
            throw new Exception("No handler found for command {0} " + query.ToString());
        }
    }
}
