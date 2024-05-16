using Domain.Auth;

namespace Application.Common.Abstractions;
public interface IUserRepository
{
    Task<User?> GetById(int id);

    Task<List<User>> GetByPredicate(Func<User, bool> predicate);

    Task<List<User>> GetAll();

    Task Add(User user);

    Task Remove(int id);

    Task SaveChanges();
}
