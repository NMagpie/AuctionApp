using Application.Abstractions;
using AutoMapper;
using EntityFramework.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistance.Repositories;
public class EntityRepository : IRepository
{
    private readonly AuctionAppDbContext _aucitonAppDbContext;
    private readonly IMapper _mapper;

    public EntityRepository(AuctionAppDbContext auctionAppDbContext, IMapper mapper)
    {
        _aucitonAppDbContext = auctionAppDbContext;
        _mapper = mapper;
    }

    public async Task Add<T>(T entity) where T : Entity
    {
        await _aucitonAppDbContext
            .Set<T>()
            .AddAsync(entity);
    }

    public async Task<List<T>> GetAll<T>() where T : Entity
    {
        return await _aucitonAppDbContext
            .Set<T>()
            .ToListAsync();
    }

    public async Task<T> GetById<T>(int id) where T : Entity
    {
        return await _aucitonAppDbContext
            .FindAsync<T>(id);
    }

    public async Task<T> Remove<T>(int id) where T : Entity
    {
        var entity = await _aucitonAppDbContext
            .Set<T>()
            .FindAsync(id);

        if (entity == null)
        {
            throw new ValidationException($"Object of type {typeof(T)} with id {id} not found");
        }

        _aucitonAppDbContext.Set<T>().Remove(entity);

        return entity;
    }
}
