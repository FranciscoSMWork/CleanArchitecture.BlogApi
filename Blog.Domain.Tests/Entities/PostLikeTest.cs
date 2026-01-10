using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Tests.Entities;

public class PostLikeTest
{
    // Usuário pode curtir um post
    [Fact]
    public async Task CreatePostLike_WhenDatesAreCorrect_ShouldReturnPostLike()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";
        User user = new User(userName, emailCreated, bio);

        string title = "Post Title";
        string content = "Post Content";
        Post post = new Post(title, content, user);

        //Act
        PostLike postLike = new PostLike(user, post);

        //Assert
        Assert.NotNull(postLike);
        Assert.Equal(user.Id, postLike.User.Id);
        Assert.Equal(post.Id, postLike.Post.Id);
    }

    // Usuário não pode curtir o mesmo post mais de uma vez

    // Usuário pode remover a curtida de um post -> Camada de aplicaçãp


    // Erro caso não exista curtida para remover -> Camada de aplicação

    // Erro caso tentar curtir um post inexistente
    [Fact]
    public async Task CreatePost_WhenPostIsEmpty_ShouldReturnError()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";
        User user = new User(userName, emailCreated, bio);

        Post post = null;

        //Act
        Action act = () => new PostLike(user, post);

        //Assert
        act.Should()
            .Throw<ValueCannotBeEmptyException>("Post")
            .WithMessage("Post cannot be empty.");
    }

    // Erro caso usuário não for informado
    [Fact]
    public async Task CreatePost_WhenUserIsEmpty_ShouldReturnError()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";
        User user = new User(userName, emailCreated, bio);

        User notExistingUser = null;

        string title = "Post Title";
        string content = "Post Content";
        Post post = new Post(title, content, user);

        //Act
        Action act = () => new PostLike(notExistingUser, post);

        //Assert
        act.Should()
            .Throw<ValueCannotBeEmptyException>("Author")
            .WithMessage("Author cannot be empty.");
    }
}
