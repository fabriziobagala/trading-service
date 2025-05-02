using Serilog;

namespace TradingService.API.Extensions;

internal static class HostBuilderExtensions
{
    /// <summary>
    /// Sets up Serilog for logging in the host builder.
    /// </summary>
    /// <param name="host">The host builder to configure.</param>
    /// <returns>The <see cref="IHostBuilder"/> so that additional calls can be chained.</returns>
    public static IHostBuilder UseSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, _, configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(context.Configuration["Seq:ServerUrl"]!);
        });

        return host;
    }
}
