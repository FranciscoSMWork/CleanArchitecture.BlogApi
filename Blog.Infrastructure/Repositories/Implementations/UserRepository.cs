using Blog.Application.Interfaces;
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

    public async Task<bool> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        return await _context.SaveChangesAsync() > 0;
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

    public async Task<bool> UpdateAsync(User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id && u.DeletedAt == null);

        if (existingUser == null)
            return false;

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Bio = user.Bio;

        return await _context.SaveChangesAsync() > 0;
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
