using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Implementations;

public class PostRepository : IPostRepository
{
    private readonly BlogDbContext _context;

    public PostRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Post> AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post); 
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<bool> DeleteAsync(Guid PostId)
    {
        var post = await _context.Posts.FindAsync(PostId);

        if (post is null)
            return false;

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Post?> GetByIdAsync(Guid PostId)
    {
        return await _context.Posts
            .Include(p => p.Comments)
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == PostId);
    }

    public async Task<List<Post>> GetPostsByAuthorAsync(Guid UserId)
    {
        return await _context.Posts
            .Where(p => p.AuthorId == UserId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Post>> ListAllAsync()
    {
        return await _context.Posts
            .Include(p => p.Author)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> UpdatePostAsync(Post post)
    {
        bool postExists = await _context.Posts.AnyAsync(p => p.Id == post.Id && p.DeletedAt == null);

        if (!postExists)
            return false;

        _context.Posts.Update(post);
        return await _context.SaveChangesAsync() > 0;
    }

}
