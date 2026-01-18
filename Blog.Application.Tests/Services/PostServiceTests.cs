using Blog.Application.Services;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.ValueObjects;
using Moq;

namespace Blog.Application.Tests.Services;
public class PostServiceTests
{

    private Mock<IPostRepository> _postRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private PostService _postService;

    public PostServiceTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _postService = new PostService(_postRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task SearchPostById_WhendIdIsCorrect_ShouildReturnPost()
    {
        // Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "Test Name";
        string bio = "Bio Test";

        User user = new User (nameUser, emailCreated, bio);

        string title = "Title Test";
        string content = "Content Test";

        Post post = new Post(title, content, user);

        _postRepositoryMock
            .Setup(_postRepositoryMock => _postRepositoryMock.GetByIdAsync(post.Id))
            .ReturnsAsync(post);

        // Act
        Post postReturned = await _postService.FindPostById(user.Id);

        // Assert
        _postRepositoryMock
            .Verify(_postRepositoryMock => _postRepositoryMock.GetByIdAsync(user.Id));

    }

    [Fact]
    public async Task CreatePost_WhenDatasAreCorrect_ShouldReturnTrue()
    {
        // Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "Test Name";
        string bio = "Bio Test";

        User user = new User(nameUser, emailCreated, bio);

        string title = "Title Test";
        string content = "Content Test";

        Post post = new Post(title, content, user);

        _postRepositoryMock
            .Setup(_postRepositoryMock => _postRepositoryMock.AddAsync(post))
            .ReturnsAsync(true);

        // Act
        bool postReturned = await _postService.AddAsync(post);

        // Assert
        Assert.True(postReturned);
        _postRepositoryMock
            .Verify(_postRepositoryMock => _postRepositoryMock.AddAsync(post), Times.Once);
    }

    [Fact]
    public async Task ListPost_WhenCalled_ShouldReturnPostList()
    {
        // Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "Test Name";
        string bio = "Bio Test";
        User user = new User(nameUser, emailCreated, bio);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, user);

        List<Post> postList = new List<Post> { post };

        _postRepositoryMock
            .Setup(_postRepositoryMock => _postRepositoryMock.ListAllAsync())
            .ReturnsAsync(postList);

        // Act
        List<Post> postReturned = await _postService.ListAllPostAsync();

        // Assert
        Assert.All(postReturned, p => Assert.IsType<Post>(p));
        Assert.Single(postReturned);
        _postRepositoryMock.Verify(_postRepositoryMock => _postRepositoryMock.ListAllAsync());

    }

        [Fact]
        public async Task UpdateTask_WhenDatasAreCorrect_ShouldReturnTrue()
        {
            // Arrange
            string email = "email@test.com";
            Email emailCreated = new Email(email);

            string nameUser = "Test Name";
            string bio = "Bio Test";
            User user = new User(nameUser, emailCreated, bio);

            string newNameUser = "New Test Name";
            string newBio = "New Bio Test";
            User newUser = new User(newNameUser, emailCreated, newBio);

            string title = "Title Test";
            string content = "Content Test";
            Post post = new Post(title, content, user);

            post.Content = content;
            post.Author = newUser;

            _postRepositoryMock
                .Setup(_postRepositoryMock => _postRepositoryMock.UpdatePost(post))
                .ReturnsAsync(true);
        
            // Act
            bool postUpdated =  await _postService.UpdatePostAsync(post);

            // Assert
            _postRepositoryMock
                .Verify(_postRepositoryMock => _postRepositoryMock.UpdatePost(post), Times.Once);
        }

        [Fact]
        public async Task DeletePost_WhenIdIsCorrect_ShouldReturnTrue()
        {
            // Arrange
            string email = "email@test.com";
            Email emailCreated = new Email(email);

            string nameUser = "Test Name";
            string bio = "Bio Test";
            User user = new User(nameUser, emailCreated, bio);

            string title = "Title Test";
            string content = "Content Test";
            Post post = new Post(title, content, user);

            _postRepositoryMock
                .Setup(_postRepositoryMock => _postRepositoryMock.DeleteAsync(post.Id))
                .ReturnsAsync(true);

            // Act
            bool postDeleted = await _postService.DeletePostAsync(post.Id);

            // Assert
            _postRepositoryMock
                .Verify(_postRepositoryMock => _postRepositoryMock.DeleteAsync(post.Id), Times.Once);
        }

}
