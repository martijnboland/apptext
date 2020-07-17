using AppText.Shared.Commands;
using AppText.Shared.Queries;
using System.Threading.Tasks;

namespace AppText.Shared.Infrastructure
{
    /// <summary>
    /// Mediator for dispatching queries, commands and events.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Executes the given command. A command is always handled by a single handler.
        /// </summary>
        /// <typeparam name="T">The type of the command</typeparam>
        /// <param name="command">The command to execute</param>
        /// <returns>The result of the command (status, validationerrors)</returns>
        Task<CommandResult> ExecuteCommand<T>(T command) where T : ICommand;

        /// <summary>
        /// Executes the given query.
        /// </summary>
        /// <typeparam name="TResult">The type of the query</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>The result of the query</returns>
        Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query);

        /// <summary>
        /// Publishes the given event. An event can be handled by multiple handlers.
        /// </summary>
        /// <typeparam name="T">The type of the event</typeparam>
        /// <param name="eventToPublish">The event to publish</param>
        /// <returns></returns>
        Task PublishEvent<T>(T eventToPublish) where T : IEvent;
    }
}