using KeyManager.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KeyManager.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("----- Handling request {RequestName} ({@Request})", request.GetGenericTypeName(),
            request);
        var response = await next();
        _logger.LogInformation("----- Request {RequestName} handled - response: {@Response}",
            request.GetGenericTypeName(), response);

        return response;
    }
}

