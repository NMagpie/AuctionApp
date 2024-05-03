using EntityFramework.Domain.Abstractions;
using System.Linq.Expressions;

namespace Application.Abstractions;
public interface IRepository
{
    Task<T?> GetById<T>(int id) where T : Entity;

    Task<T?> GetByIdWithInclude<T>(int id, params Expression<Func<T, object>>[] includeProperties) where T : Entity;

    Task<List<T>> GetByIds<T>(List<int> ids) where T : Entity;

    Task<List<T>> GetByPredicate<T>(Func<T, bool> predicate) where T : Entity;

    Task<List<T>> GetAll<T>() where T : Entity;

    Task Add<T>(T entity) where T : Entity;

    Task<T> Remove<T>(int id) where T : Entity;

    Task SaveChanges();
}