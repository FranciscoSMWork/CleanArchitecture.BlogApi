using Blog.Application.DTOs.Posts;
using Blog.Application.Services;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Blog.Application.Tests.Services;
public class PostServiceTests
{

    private Mock<IPostRepository> _postRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private PostService _postService;

    public PostServiceTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _postService = new PostService(_postRepositoryMock.Object, _userRepositoryMock.Object , _unitOfWorkMock.Object);
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
    public async Task CreatePost_WhenDatasAreCorrect_ShouldReturnNewPost()
    {
        // Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "Test Name";
        string bio = "Bio Test";

        User user = new User(nameUser, emailCreated, bio);

        _userRepositoryMock
            .Setup(_userRepositoryMock => _userRepositoryMock.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        string title = "Title Test";
        string content = "Content Test";

        Post post = new Post(title, content, user);

        _postRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Post>()))
            .ReturnsAsync((Post p) => p);

        CreatePostDto createPostDto = new CreatePostDto
        {
            Title = title,
            Content = content,
            AuthorId = user.Id
        };

        // Act
        PostResultDto postReturned = await _postService.AddAsync(createPostDto);

        // Assert
        postReturned.Title.Should().Be(title);
        postReturned.Content.Should().Be(content);
        postReturned.Author.Name.Should().Be(nameUser);
        postReturned.Author.Bio.Should().Be(bio);
        _postRepositoryMock
            .Verify(r => r.AddAsync(It.IsAny<Post>()), Times.Once);
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
                .Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(post);

/*            _postRepositoryMock
                .Setup(_postRepositoryMock => _postRepositoryMock.UpdatePostAsync(post))
                .ReturnsAsync(true);
*/
            UpdatePostDto updatePostDto = new UpdatePostDto
            {
                Title = post.Title,
                Content = post.Content
            };

            // Act
            Post postUpdated =  await _postService.UpdatePostAsync(post.Id, updatePostDto);

            // Assert
            _postRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
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
