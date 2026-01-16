using Blog.Domain.Entities;

namespace Blog.Application.Interfaces;
public interface ICommentRepository
{
    Task<Comment> GetByIdAsync(Guid Id);
    Task<bool> AddAsync(Comment comment);
}
