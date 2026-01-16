using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;

namespace Blog.Application.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<Boolean> AddAsync(User user);
    Task<List<User>> ListAllAsync();
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> EmailExists(Email email);
    Task<User> GetUserByEmail(Email email);
    Task<List<User>> GetAllAsync();
}
