using Asp.Versioning.ApiExplorer;
using TradingService.API.Middleware;

namespace TradingService.API.Extensions;

internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Registers Swagger middleware with versioning support.
    /// </summary>
    /// <param name="app">The application builder to configure.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
    public static IApplicationBuilder UseSwaggerWithVersioning(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // Build a Swagger endpoint for each discovered API version.
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"Trading Service API {description.ApiVersion}");
            }
        });

        return app;
    }

    /// <summary>
    /// Adds middleware for exception handling.
    /// </summary>
    /// <param name="app">The application builder to register the middleware with.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
    public static IApplicationBuilder UseUseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
