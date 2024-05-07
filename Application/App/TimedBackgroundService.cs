using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.App;

public class TimedBackgroundService : BackgroundService
{
    private readonly ILogger<TimedBackgroundService> _logger;

    private readonly IRepository _repository;

    private TimeSpan _timeout;

    public TimedBackgroundService(ILogger<TimedBackgroundService> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
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
        Func<Auction, bool> predicate = auction =>
        {
            if (auction.EndTime > DateTimeOffset.UtcNow)
                return false;

            return auction.Lots.Any(lot => lot.Bids.All(bid => !bid.IsWon));
        };

        var finishedAuctions = await _repository.GetByPredicate<Auction>(predicate, x => x.Lots, x => x.Lots.Select(l => l.Bids));

        foreach (Auction auction in finishedAuctions)
        {
            foreach (Lot lot in auction.Lots)
            {
                //think about further logic and implement
            }
        }
    }
}
