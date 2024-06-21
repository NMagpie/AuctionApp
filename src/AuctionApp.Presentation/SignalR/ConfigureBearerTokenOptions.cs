namespace AuctionApp.Presentation.SignalR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
public class ConfigureBearerTokenOptions : IPostConfigureOptions<BearerTokenOptions>
{
    public void PostConfigure(string? name, BearerTokenOptions options)
    {
        var originalOnMessageReceived = options.Events.OnMessageReceived;
        options.Events.OnMessageReceived = async context =>
        {
            await originalOnMessageReceived(context);

            if (string.IsNullOrEmpty(context.Token))
            {
                var accessToken = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
            }
        };
    }
}
