using Afdb.ClientConnection.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Afdb.ClientConnection.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Une exception non gérée s'est produite");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ValidationException validationEx => (HttpStatusCode.BadRequest,
                JsonSerializer.Serialize(new
                {
                    message = "Erreurs de validation",
                    errors = validationEx.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                })),
            FluentValidation.ValidationException fluentValidationEx => (HttpStatusCode.BadRequest,
                JsonSerializer.Serialize(new
                {
                    message = "Erreurs de validation",
                    errors = fluentValidationEx.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                })),
            NotFoundException => (HttpStatusCode.NotFound,
                JsonSerializer.Serialize(new { message = exception.Message })),
            ForbiddenAccessException => (HttpStatusCode.Forbidden,
                JsonSerializer.Serialize(new { message = exception.Message })),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized,
                JsonSerializer.Serialize(new { message = "ERR.General.NotAuthorize" })),
            InvalidOperationException => (HttpStatusCode.BadRequest,
                JsonSerializer.Serialize(new { message = exception.Message })),
            ArgumentException => (HttpStatusCode.BadRequest,
                JsonSerializer.Serialize(new { message = exception.Message })),
            _ => (HttpStatusCode.InternalServerError,
                JsonSerializer.Serialize(new { message = "ERR.General.InternalServerError" }))
        };

        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(message);
    }
}