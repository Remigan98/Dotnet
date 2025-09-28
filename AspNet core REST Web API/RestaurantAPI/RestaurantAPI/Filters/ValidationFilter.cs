using System.Threading;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestaurantAPI.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.Count == 0)
            {
                await next();
                return;
            }

            CancellationToken ct = context.HttpContext.RequestAborted;
            bool anyInvalid = false;

            foreach (KeyValuePair<string, object?> arg in context.ActionArguments)
            {
                if (arg.Value is null)
                {
                    // Let model binding nullability / required attributes handle this.
                    continue;
                }

                Type argumentType = arg.Value.GetType();
                Type validatorType = typeof(IValidator<>).MakeGenericType(argumentType);

                object? validatorObj = context.HttpContext.RequestServices.GetService(validatorType);
                if (validatorObj is null)
                    continue; // No validator registered for this argument type.

                // Use dynamic dispatch to call ValidateAsync(T, CancellationToken)
                dynamic dynamicValidator = validatorObj;
                ValidationResult result = await dynamicValidator.ValidateAsync((dynamic)arg.Value, ct);

                if (!result.IsValid)
                {
                    anyInvalid = true;
                    foreach (ValidationFailure failure in result.Errors)
                    {
                        context.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }
                }
            }

            if (anyInvalid)
            {
                ProblemDetails problem = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Extensions = { ["errors"] = context.ModelState }
                };

                context.Result = new ObjectResult(problem)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return;
            }

            await next();
        }
    }
}