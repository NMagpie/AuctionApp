using Application.Abstractions;

namespace Infrastructure.Persistance;
public class UnitOfWork : IUnitOfWork
{
    private readonly AuctionAppDbContext _auctionAppDbContext;

    public UnitOfWork(AuctionAppDbContext auctionAppDbContext, IRepository repository)
    {
        Repository = repository;
        _auctionAppDbContext = auctionAppDbContext;
    }

    public IRepository Repository { get; private set; }

    public async Task SaveChanges()
    {
        await _auctionAppDbContext.SaveChangesAsync();
    }
}
