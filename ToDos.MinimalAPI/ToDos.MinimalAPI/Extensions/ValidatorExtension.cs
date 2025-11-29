using FluentValidation;

namespace ToDos.MinimalAPI.Extensions
{
    public static class ValidatorExtension
    {
        public static RouteHandlerBuilder WithValidator<T>(this RouteHandlerBuilder builder)
            where T : class
        {
            builder.Add(endpointBuilder => 
            {
                RequestDelegate? originalDelegate = endpointBuilder.RequestDelegate;
                endpointBuilder.RequestDelegate = async httpContext =>
                {
                    IValidator<T> validator = httpContext.RequestServices.GetRequiredService<IValidator<T>>();

                    httpContext.Request.EnableBuffering();

                    T? body = await httpContext.Request.ReadFromJsonAsync<T>();

                    if (body is null)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                        await httpContext.Response.WriteAsJsonAsync("Failed to map body to request model");
                        return;
                    }

                    FluentValidation.Results.ValidationResult validationResult = await validator.ValidateAsync(body);

                    if (validationResult.IsValid == false)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                        await httpContext.Response.WriteAsJsonAsync(validationResult.Errors);
                        return;
                    }

                    httpContext.Request.Body.Position = 0;

                    await originalDelegate!(httpContext);
                };
            });

            return builder;
        }
    }
}
