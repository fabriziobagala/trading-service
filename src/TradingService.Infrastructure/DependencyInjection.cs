using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingService.Application.Common.Interfaces.Persistence;
using TradingService.Domain.Repositories;
using TradingService.Infrastructure.Persistence;
using TradingService.Infrastructure.Persistence.Context;
using TradingService.Infrastructure.Persistence.Repositories;

namespace TradingService.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Adds services required for the infrastructure layer.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration object containing application settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistence(configuration.GetConnectionString("DefaultConnection"));

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        string? connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<TradingDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITradeRepository, TradeRepository>();

        return services;
    }
}
