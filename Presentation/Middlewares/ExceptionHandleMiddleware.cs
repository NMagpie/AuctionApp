using FluentValidation;
using OnlineBookShop.API.Middlewares.Models;
using System.Net;

namespace Presentation.Middlewares;

public class ExceptionHandleMiddleware
{

    private readonly ILogger _logger;

    private readonly RequestDelegate _next;

    public ExceptionHandleMiddleware(ILogger<ExceptionHandleMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case ValidationException _:
                    ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            await CreateExceptionResponseAsync(ctx, ex);

        }
    }

    private static Task CreateExceptionResponseAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message
        }.ToString());
    }
}
