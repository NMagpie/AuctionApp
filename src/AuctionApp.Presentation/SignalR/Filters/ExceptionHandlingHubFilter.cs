using Microsoft.AspNetCore.SignalR;

namespace AuctionApp.Presentation.SignalR.Filters;
public class ExceptionHandlingHubFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(invocationContext, ex);
            return default;
        }
    }

    private Task HandleExceptionAsync(HubInvocationContext context, Exception exception)
    {
        var errorMessage = "An error occurred: " + exception.Message;
        return context.Hub.Clients.Client(context.Context.ConnectionId).SendAsync("ReceiveError", errorMessage);
    }
}