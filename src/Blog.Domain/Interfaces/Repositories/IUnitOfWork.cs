namespace Blog.Domain.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync();
}
