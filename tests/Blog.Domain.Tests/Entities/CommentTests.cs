using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;
using FluentAssertions;

namespace Blog.Domain.Tests.Entities;

public class CommentTests
{
    //Criar comentário válido
    [Fact]
    public async Task CreateComment_WHenDataIsValid_ShouldReturnNewComment()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string name = "Name Test";
        string bio = "Bio Test";
        User author = new User(name, emailCreated, bio);
        
        string title = "Title Test";
        string content = "Content Test";

        Post post = new Post(title, content, author);

        //Act
        Comment comment = new Comment(post, author, content);

        //Assert
        Assert.NotNull(comment);
        Assert.Equal(post.Id, comment.Post.Id);
        Assert.Equal(author.Id, comment.Author.Id);
        Assert.Equal(content, comment.Content);
        Assert.True(comment.CreatedAt <= DateTime.UtcNow);
    }

    //Não permitir comentário vazio
    [Fact]
    public async Task CreateComment_WhenCommentIsEmpty_ShouldReturnError()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string name = "Name Test";
        string bio = "Bio Test";
        User author = new User(name, emailCreated, bio);

        string title = "Title Test";
        string content = "";

        Post post = new Post(title, content, author);

        //Act
        Action act = () => new Comment(post, author, content);

        //Assert
        act.Should()
            .Throw<ValueCannotBeEmptyException>("Content")
            .WithMessage("Content cannot be empty.");
    }

    //Comentário deve ter autor
    [Fact]
    public async Task CreateComment_WhenAuthorIsEmpty_ShouldReturnError()
    {
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string name = "Name Test";
        string bio = "Bio Test";
        User authorPost = new User(name, emailCreated, bio);

        User authorComment = null!;

        string title = "Title Test";
        string content = "Content test";
        Post post = new Post(title, content, authorPost);

        //Act
        Action act = () => new Comment(post, authorComment, content);

        //Assert
        act.Should()
            .Throw<ValueCannotBeEmptyException>("Author")
            .WithMessage("Author cannot be empty.");
    }

    //Comentário deve pertencer a um post
    [Fact]
    public async Task CreateComment_WhenPostIsEmpty_ShoulReturnError()
    {
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string name = "Name Test";
        string bio = "Bio Test";
        User author = new User(name, emailCreated, bio);

        string content = "Content test";

        Post post = null!;

        //Act
        Action act = () => new Comment(post, author, content);

        //Assert
        act.Should()
            .Throw<ValueCannotBeEmptyException>("Post")
            .WithMessage("Post cannot be empty.");
    }
}
