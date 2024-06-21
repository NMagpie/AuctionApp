using Infrastructure.Persistance;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AuctionApp.Presentation.SignalR.Filters;

public class TransactionHandlingHubFilter : IHubFilter
{

    private readonly AuctionAppDbContext _context;

    public TransactionHandlingHubFilter(AuctionAppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object>> next
        )
    {
        using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var result = await next(invocationContext);

        await _context.Database.CommitTransactionAsync();

        return result;
    }
}