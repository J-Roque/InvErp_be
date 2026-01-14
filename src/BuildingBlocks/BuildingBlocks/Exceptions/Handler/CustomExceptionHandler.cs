using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurrence {time}",
            exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode, string? CustomMessage) details = exception switch
        {
            InternalServerException ex =>
            (
                ex.Message,
                ex.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError,
                ex.CustomMessage ?? ex.Message
            ),
            ValidationException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest,
                exception.Message
            ),
            BadRequestException ex =>
            (
                ex.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest,
                ex.CustomMessage ?? ex.Message
            ),
            BadHttpRequestException ex =>
            (
                ex.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest,
                ex.Message
            ),
            UnauthorizedException ex =>
            (
                ex.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status401Unauthorized,
                ex.CustomMessage ?? ex.Message
            ),
            NotFoundException ex =>
            (
                ex.Message,
                ex.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status404NotFound,
                ex.CustomMessage ?? ex.Message
            ),
            ForbiddenException ex =>
            (
                ex.Message,
                ex.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status403Forbidden,
                ex.CustomMessage ?? ex.Message
            ),
            _ =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError,
                exception.Message
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (details.CustomMessage is not null)
        {
            problemDetails.Extensions.Add("message", details.CustomMessage);
        }

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("validationErrors", validationException.Errors);
        }

        context.Response.StatusCode = details.StatusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}
