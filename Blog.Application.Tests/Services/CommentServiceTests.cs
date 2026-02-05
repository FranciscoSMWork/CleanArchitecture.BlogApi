using Blog.Application.DTOs.Comments;
using Blog.Application.Services;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Blog.Application.Tests.Services;
public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CommentService _commentService;
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public CommentServiceTests()
    {
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _commentService = new CommentService(
            _commentRepositoryMock.Object,
            _userRepositoryMock.Object,
            _postRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async void SearchComment_WhenIdExists_ShouldReturnComment()
    {
        // Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "Test Name";
        string bio = "Bio Test";
        User user = new User(nameUser, emailCreated, bio);

        string title = "Title Test";
        string contentPost = "Content Test";
        Post post = new Post(title, contentPost, user);

        string contentComment = "Comment Test";
        Comment comment = new Comment(post, user, contentComment);

        _commentRepositoryMock
            .Setup(_commentRepositoryMock => _commentRepositoryMock.GetByIdAsync(comment.Id))
            .ReturnsAsync(comment);

        // Act
        Comment commentReturned = await _commentService.FindCommentById(comment.Id);

        // Assert
        _commentRepositoryMock
            .Verify(_commentRepositoryMock => _commentRepositoryMock.GetByIdAsync(comment.Id), Times.Once);
        commentReturned.Content.Should().Be(contentComment);
        commentReturned.Author.Name.Should().Be(nameUser);
        commentReturned.Post.Content.Should().Be(contentPost);

    }


    [Fact]
    public async void CreateComment_WhenDatasAreCorrect_ShouldReturnTrue()
    {
        // Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "Test Name";
        string bio = "Bio Test";
        User user = new User(nameUser, emailCreated, bio);

        string title = "Title Test";
        string contentPost = "Content Test";
        Post post = new Post(title, contentPost, user);

        string contentComment = "Comment Test";
        Comment comment = new Comment(post, user, contentComment);

        _commentRepositoryMock
            .Setup(_commentRepositoryMock => _commentRepositoryMock.AddAsync(comment))
            .ReturnsAsync(comment);


        CommentCreateDto commentCreate = new CommentCreateDto
        {
            PostId = post.Id,
            AuthorId = user.Id,
            Content = contentComment
        };

        //Act
        CommentDto commentAdded = await _commentService.AddCommentAsync(commentCreate);

        //Assert
        _commentRepositoryMock.Verify(_commentRepositoryMock => _commentRepositoryMock.AddAsync(comment), Times.Once);
        
    }

}
