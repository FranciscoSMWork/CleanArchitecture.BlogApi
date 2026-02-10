using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Implementations;
public class UserRepository : IUserRepository
{

    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(Guid UserId)
    {
        var user = await _context.Users.FindAsync(UserId);

        if (user is null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User?> GetByIdAsync(Guid UserId)
    {
        return await _context.Users
            .Include(u => u.Comments)
            .Include(u => u.LikedPosts)
            .Include(u => u.Posts)
            .FirstOrDefaultAsync(u => u.Id == UserId);
    }

    public async Task<List<User>> ListAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User> GetUserByEmail(Email email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExists(Email email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

}
