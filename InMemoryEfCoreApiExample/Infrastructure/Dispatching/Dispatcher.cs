using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Dispatching
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<TResult> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            Type commandType = command.GetType();
            Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

            dynamic? handler = _serviceProvider.GetRequiredService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for command type {commandType.Name}");
            }

            // Get all pipeline behaviors for this request type
            Type behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(commandType, typeof(TResult));
            IEnumerable<dynamic> behaviors = _serviceProvider.GetServices(behaviorType)!;

            // Build the pipeline
            RequestHandlerDelegate<TResult> handlerDelegate = async () =>
            {
                return await handler.Handle((dynamic)command, cancellationToken);
            };

            // Chain behaviors in reverse order
            foreach (dynamic behavior in behaviors.Reverse())
            {
                RequestHandlerDelegate<TResult> next = handlerDelegate;
                handlerDelegate = async () =>
                {
                    return await behavior.Handle((dynamic)command, next, cancellationToken);
                };
            }

            // Execute the pipeline
            return await handlerDelegate();
        }

        public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            Type queryType = query.GetType();
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));

            dynamic? handler = _serviceProvider.GetRequiredService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for query type {queryType.Name}");
            }

            // Get all pipeline behaviors for this request type
            Type behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(queryType, typeof(TResult));
            IEnumerable<dynamic> behaviors = _serviceProvider.GetServices(behaviorType)!;

            // Build the pipeline
            RequestHandlerDelegate<TResult> handlerDelegate = async () =>
            {
                return await handler.Handle((dynamic)query, cancellationToken);
            };

            // Chain behaviors in reverse order
            foreach (dynamic behavior in behaviors.Reverse())
            {
                RequestHandlerDelegate<TResult> next = handlerDelegate;
                handlerDelegate = async () =>
                {
                    return await behavior.Handle((dynamic)query, next, cancellationToken);
                };
            }

            // Execute the pipeline
            return await handlerDelegate();
        }
    }
}