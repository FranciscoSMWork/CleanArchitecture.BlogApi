using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Repositories.Implementations;
using Blog.Infrastructure.Tests.Fixtures;
using FluentAssertions;

namespace Blog.Infrastructure.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly BlogDbContext _context;
    private readonly UserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UserRepositoryTests()
    {
        _context = DbContextFactory.CreateSqlServerInMemory();
        _context.Database.EnsureCreated();

        _repository = new UserRepository(_context);

        _unitOfWork = new UnitOfWork(_context);
    }

    //Salvar usuário
    [Fact]
    public async Task CreateUser_WhenDatasIsCorrect_ShouldReturnUserCreated()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameCreated = "Nome Criado";
        string bioTest = "Bio Test";
        User user = new User(nameCreated, emailCreated, bioTest);

        //Act
        await _repository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        //Assert
        User? userReturned = await _context.Users.FindAsync(user.Id);
        userReturned.Should().NotBeNull();
        userReturned.Name.Should().Be(nameCreated);
        userReturned.Bio.Should().Be(bioTest);
    }

    //Selecionar Usuário
    public async Task SelectUser_WhenIdExists_ShouldReturnUser()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameCreated = "Nome Criado";
        string bioTest = "Bio Test";
        User user = new User(nameCreated, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, user);

        string contentComment = "Content";
        Comment comment = new Comment(post, user, contentComment);

        await _context.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        //Act
        User? userReturned = await _repository.GetByIdAsync(user.Id);

        //Assert
        userReturned.Should().NotBeNull();
        userReturned.Name.Should().Be(nameCreated);
        userReturned.Bio.Should().Be(bioTest);

        foreach(Post postReturned in userReturned.Posts)
        {
            postReturned.Title.Should().Be(title);
            postReturned.Content.Should().Be(content);
        }
        
        foreach(Comment commentReturned in userReturned.Comments)
        {
            commentReturned.Content.Should().Be(contentComment);
        }
    }

    //Listar Todos os Usuários
    [Fact]
    public async Task ListUsers_WhenDatasAreCorrect_ShouldReturnAllUsers()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameCreated = "Nome Criado";
        string bioTest = "Bio Test";
        User user = new User(nameCreated, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, user);

        string contentComment = "Content";
        Comment comment = new Comment(post, user, contentComment);

        await _context.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        //Act
        List<User> usersReturned = await _repository.ListAllAsync();

        //Assert
        usersReturned.Should().NotBeNull();
        Assert.Single(usersReturned);
        foreach (User userReturned in usersReturned)
        {
            userReturned.Name.Should().Be(user.Name);
            userReturned.Bio.Should().Be(user.Bio);
        }
    }

    //Buscar usuário por email
    [Fact]
    public async Task FindUserByEmail_WhenEmailIsCorrect_ShouldReturnUser()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameCreated = "Nome Criado";
        string bioTest = "Bio Test";
        User user = new User(nameCreated, emailCreated, bioTest);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, user);

        string contentComment = "Content";
        Comment comment = new Comment(post, user, contentComment);

        await _context.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        //Act
        User? userReturned = await _repository.GetUserByEmail(emailCreated);

        //Assert
        userReturned.Should().NotBeNull();
        userReturned.Name.Should().Be(user.Name);
        userReturned.Bio.Should().Be(user.Bio);

    }

    //EmailExistsAsync retorna true
    [Fact]
    public async Task VerifyEmail_IfEmailExists_ShouldReturnTrue()
    {
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Name Test";
        string bioUser = "User Bio";
        User user = new User(nameUser, emailCreated, bioUser);

        await _context.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        //Act
        bool emailExists = await _repository.EmailExists(user.Email);

        //Assert
        emailExists.Should().BeTrue();
    }

    //EmailExistsAsync retorna false
    [Fact]
    public async Task VerifyEmail_IfEmailDoesNotExists_ShouldReturnFalse()
    {
        string email = "email@test.com";
        Email emailCreatedNotExists = new Email(email);

        //Act
        bool emailExists = await _repository.EmailExists(emailCreatedNotExists);

        //Assert
        emailExists.Should().BeFalse();
    }

    //Atualizar Usuário
    [Fact]
    public async Task UpdateUser_WhenDateAreCorrect_ShouldReturnTrue()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Name Test";
        string bioUser = "User Bio";
        User user = new User(nameUser, emailCreated, bioUser);

        await _context.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        string newEmail = "newemail@test.com";
        Email newCreatedEmail = new Email(newEmail);
        
        string newNameUser = "User Name Test";
        string newBioUser = "User Bio";

        user.Name = newNameUser;
        user.Bio = newBioUser;
        user.Email = newCreatedEmail;

        //Act
        bool userUpdated = await _repository.UpdateAsync(user);

        //Assert
        userUpdated.Should().BeTrue();
    }

    //Deletar Usuário
    [Fact]
    public async Task DeleteUser_WhenDateAreCorrect_ShouldReturnTrue()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Name Test";
        string bioUser = "User Bio";
        User user = new User(nameUser, emailCreated, bioUser);

        await _context.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        //Act
        bool userUpdated = await _repository.DeleteAsync(user.Id);

        //Assert
        userUpdated.Should().BeTrue();
    }

}
