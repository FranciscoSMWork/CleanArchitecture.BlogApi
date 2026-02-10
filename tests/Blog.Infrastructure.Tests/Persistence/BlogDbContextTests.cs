using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Windows.Markup;

namespace Blog.Infrastructure.Tests.Persistence;

public class BlogDbContextTests
{
    private readonly BlogDbContext _context;
    public BlogDbContextTests()
    {
        _context = DbContextFactory.CreateSqliteInMemory();
        _context.Database.EnsureCreated();
    }

    //Criar banco em memória
    [Fact]
    public async Task Should_Create_Database_InMemory()
    {
        _context.Database.CanConnect().Should().BeTrue();
    }

    //Aplicar migrations com sucesso
/*    [Fact]
    public async Task Should_Run_Migration_Successfully()
    {
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BlogV2;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

        using var context = new BlogDbContext(options);

        context.Database.EnsureDeleted();

        Action act = () => context.Database.Migrate();

        act.Should().NotThrow();
    }
*/
    //Persistir User do domínio
    [Fact]
    public async Task Should_Persist_Domain_User()
    {
        //Arrange
        var email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "User Test";
        string bio = "Bio Test";
        User user = new User(userName, emailCreated, bio);

        //Act
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        //Assert
        savedUser.Should().NotBeNull();
        savedUser!.Name.Should().Be(userName);
        savedUser!.Email.Address.Should().Be(email);
    }

    //Persistir Post com relacionamento
    [Fact]
    public async Task SavePost_WhenDbIsConfigured_ShouldPersistPost()
    {
        //Arrange
        var email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "User Test";
        string bio = "Bio Test";
        User user = new User(userName, emailCreated, bio);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, user);

        //Act
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        //Assert
        var savedPost = await _context.Posts
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == post.Id);

        savedPost.Should().NotBeNull();
        savedPost!.Title.Should().Be(title);
        savedPost!.Content.Should().Be(content);

    }
}
