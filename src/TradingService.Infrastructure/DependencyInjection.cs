using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingService.Application.Common.Interfaces.Caching;
using TradingService.Application.Common.Interfaces.Messaging;
using TradingService.Application.Common.Interfaces.Persistence;
using TradingService.Domain.Repositories;
using TradingService.Infrastructure.Caching.Redis;
using TradingService.Infrastructure.Messaging.Kafka;
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

        services.AddRedis(options =>
        {
            options.ConnectionString = configuration["Redis:ConnectionString"];
            options.InstanceName = configuration["Redis:InstanceName"];
        });

        services.AddKafka(options =>
        {
            options.BootstrapServers = configuration["Kafka:BootstrapServers"];
            options.TradeExecutedTopic = configuration["Kafka:TradeExecutedTopic"];
        });

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

    private static IServiceCollection AddRedis(
        this IServiceCollection services,
        Action<RedisOptions> configureOptions)
    {
        var redisOptions = new RedisOptions();
        configureOptions(redisOptions);
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisOptions.ConnectionString;
            options.InstanceName = redisOptions.InstanceName;
        });

        services.AddDistributedMemoryCache();
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }

    private static IServiceCollection AddKafka(
        this IServiceCollection services,
        Action<KafkaOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddSingleton<IMessageProducer, KafkaProducer>();

        return services;
    }
}
