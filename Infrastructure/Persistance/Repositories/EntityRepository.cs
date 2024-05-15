using Application.Common.Abstractions;
using Application.Common.Exceptions;
using EntityFramework.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistance.Repositories;
public class EntityRepository : IEntityRepository
{
    private readonly AuctionAppDbContext _auctionAppDbContext;

    public EntityRepository(AuctionAppDbContext auctionAppDbContext)
    {
        _auctionAppDbContext = auctionAppDbContext;
    }

    public Task<T?> GetById<T>(int id) where T : Entity
    {
        return _auctionAppDbContext.FindAsync<T>(id).AsTask();
    }

    public async Task<T?> GetByIdWithInclude<T>(int id, params Expression<Func<T, object>>[] includeProperties) where T : Entity
    {
        var query = IncludeProperties(includeProperties);

        return await query.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public Task<List<T>> GetByIds<T>(List<int> ids) where T : Entity
    {
        IQueryable<T> query = _auctionAppDbContext
            .Set<T>()
            .AsQueryable()
            .Where(e => ids.Contains(e.Id));

        return query.ToListAsync();
    }

    public Task<List<T>> GetByPredicate<T>(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties) where T : Entity
    {
        IQueryable<T> query = IncludeProperties(includeProperties)
            .Where(e => predicate(e));

        return query.ToListAsync();
    }

    public Task<List<T>> GetAll<T>() where T : Entity
    {
        return _auctionAppDbContext
            .Set<T>()
            .ToListAsync();
    }

    public Task Add<T>(T entity) where T : Entity
    {
        return _auctionAppDbContext
            .Set<T>()
            .AddAsync(entity)
            .AsTask();
    }

    public async Task Remove<T>(int id) where T : Entity
    {
        var entity = await _auctionAppDbContext
            .Set<T>()
            .FindAsync(id)
        ?? throw new EntityNotFoundException($"Object of type {typeof(T)} with id {id} not found");

        _auctionAppDbContext.Set<T>().Remove(entity);
    }

    public Task RemoveRange<T>(List<int> ids) where T : Entity
    {
        IQueryable<T> query = _auctionAppDbContext
            .Set<T>()
            .AsQueryable()
            .Where(e => ids.Contains(e.Id));

        _auctionAppDbContext.Set<T>().RemoveRange(query);
        return Task.CompletedTask;
    }

    public Task SaveChanges()
    {
        return _auctionAppDbContext.SaveChangesAsync();
    }

    private IQueryable<T> IncludeProperties<T>(params Expression<Func<T, object>>[] includeProperties) where T : Entity
    {
        IQueryable<T> entities = _auctionAppDbContext.Set<T>();
        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }
        return entities;
    }
}
