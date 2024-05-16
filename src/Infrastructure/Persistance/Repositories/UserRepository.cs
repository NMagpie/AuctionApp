using Application.Common.Abstractions;
using Application.Common.Exceptions;
using Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AuctionAppDbContext _auctionAppDbContext;

    public UserRepository(AuctionAppDbContext auctionAppDbContext)
    {
        _auctionAppDbContext = auctionAppDbContext;
    }

    public Task<User?> GetById(int id)
    {
        return _auctionAppDbContext.FindAsync<User>(id).AsTask();
    }

    public Task<List<User>> GetByPredicate(Func<User, bool> predicate)
    {
        IQueryable<User> query = _auctionAppDbContext
            .Set<User>()
            .AsQueryable()
            .Where(e => predicate(e));

        return query.ToListAsync();
    }

    public Task<List<User>> GetAll()
    {
        return _auctionAppDbContext
            .Set<User>()
            .ToListAsync();
    }

    public Task Add(User user)
    {
        return _auctionAppDbContext
            .Set<User>()
            .AddAsync(user)
            .AsTask();
    }

    public async Task Remove(int id)
    {
        var entity = await _auctionAppDbContext
            .Set<User>()
            .FindAsync(id)
        ?? throw new EntityNotFoundException($"Object of type {typeof(User)} with id {id} not found");

        _auctionAppDbContext.Set<User>().Remove(entity);
    }

    public Task SaveChanges()
    {
        return _auctionAppDbContext.SaveChangesAsync();
    }
}
