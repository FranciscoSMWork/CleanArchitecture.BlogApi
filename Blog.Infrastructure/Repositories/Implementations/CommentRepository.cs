using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Infrastructure.Context;

namespace Blog.Infrastructure.Repositories.Implementations;
public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _context;

    public CommentRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Comment> GetByIdAsync(Guid Id)
    {
        return await _context.Comments.FindAsync(Id);
    }
}
