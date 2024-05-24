using Application.Common.Exceptions;
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
            ctx.Response.StatusCode = ex switch
            {
                ValidationException _ => (int)HttpStatusCode.BadRequest,
                EntityNotFoundException _ => (int)HttpStatusCode.NotFound,
                InvalidUserException _ => (int)HttpStatusCode.Forbidden,
                _ => (int)HttpStatusCode.InternalServerError,
            };
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
