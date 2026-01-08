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

public class UserTests
{
    //Criar usuário com dados válidos
    [Fact]
    public async Task CreateUser_WhenDataIsValid()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";

        //Act
        User user = new User(userName, emailCreated, bio);

        //Assert
        Assert.NotNull(user);
        Assert.Equal(userName, user.Name);
        Assert.Equal(bio, user.Bio);
        Assert.Equal(email, user.Email.Address);
    }

    //Atualizar bio com sucesso
    [Fact]
    public async Task UpdateUserBio_WhenDataIsValid()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";
        User user = new User(userName, emailCreated, bio);

        string newBio = "New Bio Test";

        //Act
        user.UpdateBio(newBio);

        //Assert
        user.Bio.Should().Be(newBio);
    }

    //Não permitir bio acima do limite
    [Fact]
    public async Task UpdateUserBio_WhenBioExceedCaracters_ReturnError()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";
        User user = new User(userName, emailCreated, bio);

        string newBio = new string('a', 1001);

        //Act
        Action act = () => user.UpdateBio(newBio);

        //Assert
        act.Should()
            .Throw<ExceedCaractersNumberException>("Bio")
            .WithMessage("Number of caracthers of Bio exceed the limit");
    }

    //Usuário deve ter ID válido
    [Fact]
    public async Task CreateUser_ShouldGenerateValidId()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";
        
        //Act
        User user = new User(userName, emailCreated, bio);

        //Assert
        user.Id.Should().NotBe(Guid.Empty);
    }
}