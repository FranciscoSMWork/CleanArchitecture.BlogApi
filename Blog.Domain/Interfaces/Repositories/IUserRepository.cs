using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;

namespace Blog.Domain.Interfaces.Repositories;
public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
    Task<List<User>> ListAllAsync();
    Task<bool> DeleteAsync(Guid id);
    Task<bool> EmailExists(Email email);
    Task<User> GetUserByEmail(Email email);
    Task<List<User>> GetAllAsync();
}
