using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    readonly ILogger<ErrorHandlingMiddleware> logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException badRequestException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { Error = badRequestException.Message });
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { Error = notFoundException.Message });
        }
        catch (ForbidException forbidException)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new { Error = forbidException.Message });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { Error = "Something went wrong. Please try again later." });
        }
    }
}