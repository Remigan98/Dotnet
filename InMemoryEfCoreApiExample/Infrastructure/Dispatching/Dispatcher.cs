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

            // Get validator if exists
            Type validatorType = typeof(IValidator<>).MakeGenericType(commandType);
            dynamic? validator = _serviceProvider.GetService(validatorType);

            // Validate if validator exists
            if (validator != null)
            {
                validator.Validate((dynamic)command);
            }

            return await handler.Handle((dynamic)command, cancellationToken);
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

            return await handler.Handle((dynamic)query, cancellationToken);
        }
    }
}