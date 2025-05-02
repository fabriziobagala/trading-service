using MediatR;
using Microsoft.Extensions.Logging;
using TradingService.Application.Logging;

namespace TradingService.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogRequestHandling(typeof(TRequest).Name, request);
        var response = await next(cancellationToken);
        _logger.LogRequestHandled(typeof(TRequest).Name);

        return response;
    }
}
