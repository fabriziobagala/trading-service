using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using TradingService.API.Middleware;

namespace TradingService.API.Extensions;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds API versioning and Swagger services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddApiVersioningAndSwagger(this IServiceCollection services)
    {
        services.ConfigureApiVersioning();
        services.ConfigureVersionedSwaggerGen();

        return services;
    }

    /// <summary>
    /// Adds services for creation of problem details for the API.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    internal static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();

        return services;
    }

    private static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static IServiceCollection ConfigureVersionedSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Add a SwaggerDoc for each discovered API version.
            var provider = services.BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();
            
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"Trading Service API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "A microservice for managing trading operations"
                    });
            }
        });

        return services;
    }
}
