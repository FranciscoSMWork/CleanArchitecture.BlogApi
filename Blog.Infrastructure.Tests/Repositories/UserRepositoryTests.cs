using Blog.Infrastructure.Context;
using Blog.Infrastructure.Tests.Fixtures;
using Microsoft.EntityFrameworkCore.Internal;

namespace Blog.Infrastructure.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly BlogDbContext _context;

    public UserRepositoryTests()
    {
        _context = DbContextFactory.CreateSqlServerInMemory();
        _context.Database.EnsureCreated();
    }

    

}
