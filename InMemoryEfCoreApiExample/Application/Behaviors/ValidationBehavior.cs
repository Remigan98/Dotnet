using Application.Abstractions;
using Application.Common.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationBehavior(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Get validator for the request type
            Type validatorType = typeof(IValidator<>).MakeGenericType(typeof(TRequest));
            dynamic? validator = _serviceProvider.GetService(validatorType);

            // If validator exists, validate the request
            if (validator != null)
            {
                try
                {
                    validator.Validate((dynamic)request);
                }
                catch (ValidationException)
                {
                    throw; // Re-throw validation exceptions
                }
                catch (ArgumentException)
                {
                    throw; // Re-throw argument exceptions
                }
            }

            // Continue to the next behavior or handler
            return await next();
        }
    }
}