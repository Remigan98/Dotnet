using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            object response;

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = new ErrorResponse
                    {
                        StatusCode = (int)statusCode,
                        Message = notFoundException.Message,
                        ErrorType = "NotFound"
                    };
                    break;

                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ValidationErrorResponse
                    {
                        StatusCode = (int)statusCode,
                        Message = "One or more validation errors occurred.",
                        ErrorType = "Validation",
                        Errors = new List<string> { validationException.Message }
                    };
                    break;

                case ArgumentException argumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ErrorResponse
                    {
                        StatusCode = (int)statusCode,
                        Message = argumentException.Message,
                        ErrorType = "BadRequest"
                    };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = new ErrorResponse
                    {
                        StatusCode = (int)statusCode,
                        Message = "An unexpected error occurred.",
                        ErrorType = "InternalServerError"
                    };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty;
    }

    public class ValidationErrorResponse : ErrorResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}