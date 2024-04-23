﻿using Application.Abstractions;
using EntityFramework.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistance.Repositories;
public class EntityRepository : IRepository
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

    public Task<List<T>> GetByIds<T>(List<int> ids) where T : Entity
    {
        IQueryable<T> query = _auctionAppDbContext
            .Set<T>()
            .AsQueryable()
            .Where(e => ids.Contains(e.Id));

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

    public async Task<T> Remove<T>(int id) where T : Entity
    {
        var entity = await _auctionAppDbContext
            .Set<T>()
            .FindAsync(id)
        ?? throw new ValidationException($"Object of type {typeof(T)} with id {id} not found");

        _auctionAppDbContext.Set<T>().Remove(entity);

        return entity;
    }

    public Task SaveChanges()
    {
        return _auctionAppDbContext.SaveChangesAsync();
    }
}