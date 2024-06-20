using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AuctionApp.Presentation.Middlewares;
public class TransactionMiddleware
{
    private readonly RequestDelegate _next;

    public TransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, AuctionAppDbContext dbContext)
    {
        if (httpContext.Request.Method == HttpMethod.Get.Method)
        {
            await _next(httpContext);
            return;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        await _next(httpContext);

        await dbContext.Database.CommitTransactionAsync();
    }
}
