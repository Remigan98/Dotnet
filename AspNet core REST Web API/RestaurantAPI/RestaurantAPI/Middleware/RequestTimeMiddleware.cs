using RestaurantAPI.Exceptions;
using System.Diagnostics;

namespace RestaurantAPI.Middleware;

public class RequestTimeMiddleware : IMiddleware
{
    ILogger<RequestTimeMiddleware> logger;
    Stopwatch stopwatch;

    public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
    {
        this.logger = logger;
        stopwatch = new Stopwatch();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        stopwatch.Start();
        await next.Invoke(context);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > 4000)
        {
            string message = $"{context.Request.Method} Request to {context.Request.Path} took too long: {stopwatch.ElapsedMilliseconds} ms.";
            logger.LogError(message);
        }
    }
}

