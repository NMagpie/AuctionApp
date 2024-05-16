using Presentation.Middlewares;

namespace Presentation.Extentions;
public static class MiddlewareExtentions
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder app) => app.UseMiddleware<LoggingMiddleware>();

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app) => app.UseMiddleware<ExceptionHandleMiddleware>();
}
