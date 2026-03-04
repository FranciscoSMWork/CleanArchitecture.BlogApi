using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories;
public interface ICommentRepository
{
    Task<Comment> GetByIdAsync(Guid Id);
    Task<Comment> AddAsync(Comment comment);
    Task<List<Comment>> GetAllAsync();
    Task<bool> DeleteAsync(Guid Id);
}
