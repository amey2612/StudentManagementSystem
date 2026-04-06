using System.Net;
using StudentManagement.Core.Models;
using System.Text.Json;

namespace StudentManagement.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
            context.Response.ContentType = "application/json";

            var response = new ApiResponse();

            if (exception is KeyNotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.Success = false;
                response.Message = "Resource not found.";
            }
            else if (exception is ArgumentException or ArgumentNullException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Success = false;
                response.Message = exception.Message;
            }
            else if (exception is InvalidOperationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Success = false;
                response.Message = exception.Message;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
                response.Errors = new List<string> { exception.Message };
            }

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
