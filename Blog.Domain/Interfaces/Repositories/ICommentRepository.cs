using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories;
public interface ICommentRepository
{
    Task<Comment> GetByIdAsync(Guid Id);
    Task<bool> AddAsync(Comment comment);
}
