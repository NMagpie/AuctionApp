using Application.Common.Exceptions;
using AuctionApp.Presentation.Middlewares.Models;
using FluentValidation;
using System.Net;

namespace AuctionApp.Presentation.Middlewares;

public class ExceptionHandleMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandleMiddleware(RequestDelegate next)
    {
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
                BusinessValidationException => (int)HttpStatusCode.UnprocessableEntity,
                EntityNotFoundException => (int)HttpStatusCode.NotFound,
                InvalidUserException => (int)HttpStatusCode.Forbidden,
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
