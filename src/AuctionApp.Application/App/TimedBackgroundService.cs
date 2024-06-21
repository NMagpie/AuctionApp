using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using Domain.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.App;

public class TimedBackgroundService : BackgroundService
{
    private readonly ILogger<TimedBackgroundService> _logger;

    private readonly IServiceProvider _services;

    private TimeSpan _timeout;

    public TimedBackgroundService(ILogger<TimedBackgroundService> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
        _timeout = TimeSpan.FromMinutes(1);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        using PeriodicTimer timer = new(_timeout);

        await DoWork();

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
        }
    }

    private async Task DoWork()
    {
        Expression<Func<Product, bool>> predicate = product =>
            (product.EndTime < DateTimeOffset.UtcNow) && !product.SellingFinished;

        using var scope = _services.CreateScope();

        var scopedRepository = scope.ServiceProvider.GetRequiredService<IEntityRepository>();

        var finishedProducts = await scopedRepository.GetByPredicate(predicate, p => p.Bids);

        foreach (Product product in finishedProducts)
        {
            var wonBid = product.Bids.Where(b => b.IsWon).FirstOrDefault();

            if (wonBid != null)
            {
                var wonUser = await scopedRepository.GetById<User>(wonBid.UserId);

                if (wonUser != null)
                {
                    wonUser.Balance -= wonBid.Amount;
                }
            }

            product.SellingFinished = true;

            await scopedRepository.SaveChanges();
        }
    }
}
