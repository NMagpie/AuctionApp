namespace AuctionApp.Presentation.Middlewares;
public class LoggingMiddleware
{
    private readonly ILogger _logger;

    private readonly RequestDelegate _next;

    public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext ctx)
    {
        var startTime = DateTime.UtcNow;
        await _next.Invoke(ctx);
        //_logger.LogInformation($"The request {ctx.Connection.Id}: {(DateTime.UtcNow - startTime).TotalMilliseconds} ms");
    }
}