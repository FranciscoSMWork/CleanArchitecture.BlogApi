using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;
using FluentAssertions;

namespace Blog.Domain.Tests.Entities;

public class PostTests
{
    //Criar post com título e conteúdo válidos
    [Fact]
    public async Task CreatePost_WhenDataIsValid()
    {
        //Arrange
        string title = "Post Title Test";
        string content = "Post Content Test";

        string email = "test@test.com";
        Email emailCreated = new Email(email);

        string name = "testName";
        string bio = "bioTest";

        User author = new User(name, emailCreated, bio);

        //Act
        Post post = new Post(title, content, author);

        //Assert
        Assert.NotNull(post);
        Assert.Equal(title, post.Title);
        Assert.Equal(content, post.Content);
        Assert.Equal(author, post.Author);
    }

    //Não permitir post sem autor
    [Fact]
    public async Task CreatePost_WhenAuthorIsNotDefined_ShouldReturnError()
    {
        //Arrange
        string title = "Post Title Test";
        string content = "Post Content Test";

        string email = "test@test.com";
        Email emailCreated = new Email(email);

        User user = null!;

        //Act
        Action act = () => new Post(title, content, user);

        //Assert
        act.Should()
            .Throw<ValueCannotBeEmptyException>("Author")
            .WithMessage("Author cannot be empty.");
    }

    //Não permitir título vazio
    [Fact]
    public async Task CreatePost_WhenTitleIsEmpty_ShouldReturnError()
    {
        //Arrange
        string title = "";
        string content = "Post Content Test";

        string email = "test@test.com";
        Email emailCreated = new Email(email);

        string name = "testName";
        string bio = "bioTest";

        User author = new User(name, emailCreated, bio);

        //Act
        Action act = () => new Post(title, content, author);

        //Assert
        act.Should()
            .Throw<ValueCannotBeEmptyException>("Title")
            .WithMessage("Title cannot be empty.");
    }

    //Atualizar conteúdo do post
    [Fact]
    public async Task UpdatePost_WhenContentIsCorrect_ShouldUpdate()
    {
        //Arrange
        string title = "Title";
        string content = "Post Content Test";

        string email = "test@test.com";
        Email emailCreated = new Email(email);

        string name = "testName";
        string bio = "bioTest";

        User author = new User(name, emailCreated, bio);

        Post post = new Post(title, content, author);

        string newContent = "Updated Post Content Test";

        //Act
        post.UpdateContent(newContent);

        //Assert
        post.Content.Should().Be(newContent);
    }
}
