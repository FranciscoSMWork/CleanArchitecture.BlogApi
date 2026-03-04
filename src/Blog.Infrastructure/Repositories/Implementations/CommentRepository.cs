using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Implementations;
public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _context;

    public CommentRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> GetByIdAsync(Guid Id)
    {
        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments
            .Include(c => c.Author)
            .Include(c => c.Post)
                .ThenInclude(p => p.Author)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(Guid Id)
    {
        var comment = await _context.Comments.FindAsync(Id);
        if (comment is null)
            return false;

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return true;
    }

}
