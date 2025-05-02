using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TradingService.API.Middleware;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
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

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception ex)
    {
        // Determine the type of exception and handle it accordingly
        if (ex is ValidationException validationEx)
        {
            await HandleValidationExceptionAsync(context, validationEx);
        }
        else
        {
            // Store the exception in the context for later use
            context.Items["Exception"] = ex;
            
            await HandleGenericExceptionAsync(context);
        }
    }

    private async Task HandleValidationExceptionAsync(
        HttpContext context,
        ValidationException validationEx)
    {
        var modelStateDictionary = CreateModelStateDictionary(validationEx);
        var problemDetails = _problemDetailsFactory.CreateValidationProblemDetails(context, modelStateDictionary);
        context.Response.StatusCode = problemDetails.Status!.Value;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private async Task HandleGenericExceptionAsync(HttpContext context)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(context);
        context.Response.StatusCode = problemDetails.Status!.Value;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static ModelStateDictionary CreateModelStateDictionary(ValidationException validationEx)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in validationEx.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return modelStateDictionary;
    }
}
