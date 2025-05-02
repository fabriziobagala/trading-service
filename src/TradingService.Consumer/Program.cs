using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TradingService.Consumer.Configuration;
using TradingService.Consumer.Services;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) => config.AddEnvironmentVariables())
    .UseSerilog((context, services, config) =>
    {
        config
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(context.Configuration["Seq:ServerUrl"]!);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<KafkaOptions>(context.Configuration.GetSection("Kafka"));
        services.AddHostedService<TradeConsumerService>();
    })
    .RunConsoleAsync();
