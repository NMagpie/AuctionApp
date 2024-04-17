using EntityFramework.Domain.Abstractions;

namespace Application.Abstractions;
public interface IRepository
{
    Task<T> GetById<T>(int id) where T : Entity;

    Task Add<T>(T entity) where T : Entity;

    Task<T> Remove<T>(int id) where T : Entity;

    Task<List<T>> GetAll<T>() where T : Entity;
}