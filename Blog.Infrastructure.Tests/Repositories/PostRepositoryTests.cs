using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Tests.Fixtures;
using Blog.Infrastructure.Repositories.Implementations;
using FluentAssertions;
using Blog.Domain.Interfaces.Repositories;

namespace Blog.Infrastructure.Tests.Repositories;
public class PostRepositoryTests
{

    private readonly BlogDbContext _context;
    private readonly PostRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PostRepositoryTests()
    {
        _context = DbContextFactory.CreateSqlServerInMemory();
        _context.Database.EnsureCreated();

        _repository = new PostRepository(_context);

        _unitOfWork = new UnitOfWork(_context);
    }

    [Fact]
    public async Task SelectPost_WhenIdExists_ShouldReturnPost()
    {
        //Arrange
        string email = "test@email.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Test";
        string bioTest = "Bio Test";
        User author = new User(nameUser, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, author);

        await _context.Posts.AddAsync(post);
        await _unitOfWork.CommitAsync();

        //Act
        Post postReturned = await _repository.GetByIdAsync(post.Id);

        //Assert
        Assert.NotNull(postReturned);
        postReturned.Title.Should().Be(title);
        postReturned.Content.Should().Be(content);
    }

    //Salvar post
    [Fact]
    public async Task CreatePostAndReturnIt_WhenDatasAreCorrect_ShouldReturnPost() 
    {
        //Arrange
        string email = "test@email.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Test";
        string bioTest = "Bio Test";
        User author = new User(nameUser, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, author);

        await _context.Users.AddAsync(author);
        await _context.SaveChangesAsync();
        await _unitOfWork.CommitAsync();

        //Act
        await _repository.AddAsync(post);

        //Assert
        Post? persistedPost = await _context.Posts.FindAsync(post.Id);

        persistedPost.Should().NotBeNull();
        persistedPost.Title.Should().Be(post.Title);
        persistedPost.Content.Should().Be(post.Content);
    }

    //Buscar Posts por usuário
    [Fact]
    public async Task FindPostByUserId_WhenPostIsCorrect_ShouldReturnAuthor()
    {
        //Arrange
        string email = "test@email.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Test";
        string bioTest = "Bio Test";
        User author = new User(nameUser, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, author);

        //Act
        await _context.Posts.AddAsync(post);
        await _unitOfWork.CommitAsync();

        List<Post> listPostsReturned = await _repository.GetPostsByAuthorAsync(author.Id);

        //Assert
        Assert.NotNull(listPostsReturned);
        Assert.NotEmpty(listPostsReturned);
        Assert.Single(listPostsReturned); 

        foreach(Post postReturned in listPostsReturned)
        {
            Assert.NotNull(postReturned);
            Assert.Equal(postReturned.Id, post.Id);
            postReturned.Title.Should().Be(title);
            postReturned.Content.Should().Be(content);
        }
    }

    //ListAllAsync
    [Fact]
    public async Task ListAllAsync_ShouldReturnAllPost()
    {
        //Arrange
        string email = "test@email.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Test";
        string bioTest = "Bio Test";
        User author = new User(nameUser, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, author);

        string contentComment = "Content";
        Comment comment = new Comment(post, author, contentComment);

        await _context.Posts.AddAsync(post);
        await _unitOfWork.CommitAsync();

        //Act
        List<Post> listPostsReturned = await _repository.ListAllAsync();

        //Assert
        foreach(Post postReturned in listPostsReturned)
        {
            postReturned.Author.Name.Should().Be(nameUser);
            postReturned.Author.Bio.Should().Be(bioTest);
            postReturned.Author.Email.Address.Should().Be(email);

            postReturned.Title.Should().Be(title);
            postReturned.Content.Should().Be(content);

            foreach (Comment commentReturned in postReturned.Comments)
            {
                commentReturned.Author.Name.Should().Be(nameUser);
                commentReturned.Author.Bio.Should().Be(bioTest);

                commentReturned.Post.Title.Should().Be(title);
                commentReturned.Post.Content.Should().Be(content);
            }
        }
    }

    //Delete
    [Fact]
    public async Task DeletePost_WhenIdExists_ShouldReturnTrue()
    {
        //Arrange
        string email = "test@email.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Test";
        string bioTest = "Bio Test";
        User author = new User(nameUser, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, author);

        string contentComment = "Content";
        Comment comment = new Comment(post, author, contentComment);


        await _context.Posts.AddAsync(post);
        await _unitOfWork.CommitAsync();

        //Act
        bool postDeleted = await _repository.DeleteAsync(post.Id);
        await _unitOfWork.CommitAsync();

        //Assert
        postDeleted.Should().BeTrue();
    }

    //Update
    [Fact]
    public async Task UpdatePost_WhenDatasAreCorrect_ShouldReturnTrue()
    {
        //Arrange
        string email = "test@email.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Test";
        string bioTest = "Bio Test";
        User author = new User(nameUser, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, author);

        string contentComment = "Content";
        Comment comment = new Comment(post, author, contentComment);

        await _context.Posts.AddAsync(post);
        await _unitOfWork.CommitAsync();

        //Act
        bool postDeleted = await _repository.UpdatePostAsync(post);

        //Assert
        postDeleted.Should().BeTrue();
    }

}
