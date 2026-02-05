using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories;
public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid Id);
    Task<Post> AddAsync(Post post);
    Task<List<Post>> ListAllAsync();
    Task<bool> UpdatePost(Post post);
    Task<bool> DeleteAsync(Guid Id);
    Task<List<Post>> GetPostsByAuthorAsync(Guid UserId);
}
