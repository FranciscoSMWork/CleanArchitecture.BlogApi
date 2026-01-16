using Blog.Domain.Entities;

namespace Blog.Application.Interfaces;

public interface IUserService
{
    Task<User> AddUser(User user);
    Task<User?> FindUserById(Guid id);
    Task<List<User>> ListAllUsers();
    Task<bool> Update(User user);
    Task<bool> Delete(Guid id);
}