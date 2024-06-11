using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Application.Common.Models;
using AuctionApp.Application.Extentions;
using AuctionApp.Domain.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistance.Repositories;
public class EntityRepository : IEntityRepository
{
    private readonly AuctionAppDbContext _auctionAppDbContext;

    private readonly IMapper _mapper;

    public EntityRepository(AuctionAppDbContext auctionAppDbContext, IMapper mapper)
    {
        _auctionAppDbContext = auctionAppDbContext;
        _mapper = mapper;
    }

    public Task<T?> GetById<T>(int id) where T : class, IEntity
    {
        return _auctionAppDbContext.FindAsync<T>(id).AsTask();
    }

    public async Task<T?> GetByIdWithInclude<T>(int id, params Expression<Func<T, object>>[] includeProperties) where T : class, IEntity
    {
        var query = IncludeProperties(includeProperties);

        return await query.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public Task<List<T>> GetByIds<T>(List<int> ids) where T : class, IEntity
    {
        IQueryable<T> query = _auctionAppDbContext
            .Set<T>()
            .AsQueryable()
            .Where(e => ids.Contains(e.Id));

        return query.ToListAsync();
    }

    public Task<List<T>> GetByPredicate<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IEntity
    {
        IQueryable<T> query = IncludeProperties(includeProperties)
            .Where(predicate);

        return query.ToListAsync();
    }

    public Task<List<T>> GetAll<T>() where T : class, IEntity
    {
        return _auctionAppDbContext
            .Set<T>()
            .ToListAsync();
    }

    public Task Add<T>(T entity) where T : class, IEntity
    {
        return _auctionAppDbContext
            .Set<T>()
            .AddAsync(entity)
            .AsTask();
    }

    public async Task Remove<T>(int id) where T : class, IEntity
    {
        var entity = await _auctionAppDbContext
            .Set<T>()
            .FindAsync(id)
        ?? throw new EntityNotFoundException($"Object of type {typeof(T)} with id {id} not found");

        _auctionAppDbContext.Set<T>().Remove(entity);
    }

    public Task RemoveRange<T>(List<int> ids) where T : class, IEntity
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

    public async Task<PaginatedResult<TDto>> GetPagedData<TEntity, TDto>(PagedRequest pagedRequest, params Expression<Func<TEntity, object>>[] includeProperties) 
        where TEntity : class, IEntity                                                                   
        where TDto : class
    {
        return await IncludeProperties(includeProperties)
            .CreatePaginatedResultAsync<TEntity, TDto>(pagedRequest, _mapper);
    }

    private IQueryable<T> IncludeProperties<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IEntity
    {
        IQueryable<T> entities = _auctionAppDbContext.Set<T>();
        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }
        return entities;
    }
}
