using Microsoft.Extensions.DependencyInjection;
using TradingService.Application.Features.Trades.Commands.ExecuteTrade;

namespace TradingService.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Adds services required for the application layer.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR();

        return services;
    }

    private static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ExecuteTradeCommandHandler>());
    }
}
