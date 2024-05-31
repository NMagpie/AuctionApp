using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.App;

public class TimedBackgroundService : BackgroundService
{
    private readonly ILogger<TimedBackgroundService> _logger;

    private readonly IEntityRepository _repository;

    private TimeSpan _timeout;

    public TimedBackgroundService(ILogger<TimedBackgroundService> logger, IEntityRepository repository)
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
        Expression<Func<Product, bool>> predicate = product =>
        (product.EndTime < DateTimeOffset.UtcNow) && product.Bids.All(bid => !bid.IsWon);

        var finishedProducts = await _repository.GetByPredicate<Product>(predicate);

        foreach (Product product in finishedProducts)
        {
                //think about further logic and implement
        }
    }
}
