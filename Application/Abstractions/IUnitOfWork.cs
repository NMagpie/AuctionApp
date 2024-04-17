namespace Application.Abstractions;
public interface IUnitOfWork
{
    public IRepository Repository { get; }

    Task SaveChanges();
}
