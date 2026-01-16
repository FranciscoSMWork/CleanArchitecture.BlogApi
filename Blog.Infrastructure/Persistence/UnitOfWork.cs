using Blog.Application.Interfaces;
using Blog.Infrastructure.Context;

namespace Blog.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlogDbContext _context;

    public UnitOfWork(BlogDbContext context)
    {
        _context = context; 
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
