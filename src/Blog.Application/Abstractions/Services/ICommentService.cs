
using Blog.Application.DTOs.Comments;
using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Services;

public interface ICommentService
{
    Task<Comment> FindCommentById(Guid Id);

    Task<CommentDto> AddCommentAsync(CommentCreateDto comment);
    Task<List<CommentDto>> ListCommentAsync();
    Task<bool> DeleteCommentAsync(Guid Id);
}
