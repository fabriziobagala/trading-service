using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TradingService.Application.Common.Exceptions;

namespace TradingService.API.Middleware;

internal class CustomProblemDetailsFactory : ProblemDetailsFactory
{
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        statusCode ??= StatusCodes.Status500InternalServerError;
        title ??= "An unexpected error occurred";
        type ??= "https://tools.ietf.org/html/rfc9110#section-15.6.1";
        detail ??= "An unexpected error occurred while processing your request.";
        instance ??= httpContext.Request.Path;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance,
        };

        ApplyExceptionDetails(problemDetails, httpContext);

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(modelStateDictionary);

        statusCode ??= StatusCodes.Status400BadRequest;
        type ??= "https://tools.ietf.org/html/rfc9110#section-15.5.1";
        instance ??= httpContext.Request.Path;

        return new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance,
        };
    }

    private static void ApplyExceptionDetails(
        ProblemDetails problemDetails,
        HttpContext httpContext)
    {
        // Add specific properties for NotFoundException
        if (httpContext.Items["Exception"] is NotFoundException notFoundEx)
        {
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Title = "Resource not found";
            problemDetails.Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
            problemDetails.Detail = notFoundEx.Message;
        }

        // Add inner exception message if available
        if (httpContext.Items["Exception"] is Exception { InnerException: Exception innerEx })
        {
            problemDetails.Extensions["innerException"] = innerEx.Message;
        }
    }
}
