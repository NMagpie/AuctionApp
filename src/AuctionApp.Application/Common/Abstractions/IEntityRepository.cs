using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Abstractions;
using System.Linq.Expressions;

namespace Application.Common.Abstractions;
public interface IEntityRepository
{
    Task<T?> GetById<T>(int id) where T : class, IEntity;

    Task<T?> GetByIdWithInclude<T>(int id, params Expression<Func<T, object>>[] includeProperties) where T : class, IEntity;

    Task<List<T>> GetByIds<T>(List<int> ids) where T : class, IEntity;

    Task<List<T>> GetByPredicate<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IEntity;

    Task<List<T>> GetAll<T>() where T : class, IEntity;

    Task Add<T>(T entity) where T : class, IEntity;

    Task Remove<T>(int id) where T : class, IEntity;

    Task RemoveRange<T>(List<int> ids) where T : class, IEntity;

    Task SaveChanges();

    Task<PaginatedResult<TDto>> GetPagedData<TEntity, TDto>(PagedRequest pagedRequest, params Expression<Func<TEntity, object>>[] includeProperties)
        where TEntity : class, IEntity
        where TDto : class;
}