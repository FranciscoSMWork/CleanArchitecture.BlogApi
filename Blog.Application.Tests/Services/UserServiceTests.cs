using Blog.Application.DTOs.Users;
using Blog.Application.Services;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.ValueObjects;
using Moq;

namespace Blog.Application.Tests.Services;

public class UserServiceTests
{
    // Preciso do mock de UserRepository, pois se trata de outra camada do sistema que preciso interagir, porém, eu não a tenho criada
    // Preciso do mock de UnitOfWork, pois o serviço depende dele para salvar as alterações no banco de dados
    // Variável privada de service, service fica na camada de aplicação

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userService = new UserService(_userRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    // Deve permitir retornar um funcionário pelo seu Id
    [Fact]
    public async Task SearchUserById_WhenIdIsCorrect_ShouldReturnUser()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";

        User user = new User(userName, emailCreated, bio);

        _userRepositoryMock
            .Setup(_userRepositoryMock => _userRepositoryMock.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        //Act
        await _userService.FindUserById(user.Id);

        //Assert
        _userRepositoryMock.Verify(_userRepositoryMock => _userRepositoryMock.GetByIdAsync(user.Id), Times.Once);
    }


    // Deve permitir criar um novo funcionário
    [Fact]
    public async Task CreateUser_WhenDatasAreCorrects_ShouldReturnTrue()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";

        User user = new User(userName, emailCreated, bio);

        _userRepositoryMock
            .Setup(_userRespositoryMock => _userRespositoryMock.AddAsync(user))
            .ReturnsAsync(true);

        //Act
        CreateUserDto createUserDto = new CreateUserDto
        {
            Name = userName,
            Email = email,
            Bio = bio
        };

        var userAddedCorrectly = await _userService.AddUser(createUserDto);

        //Assert
        _userRepositoryMock.Verify(_userRespositoryMock => _userRespositoryMock.AddAsync(user), Times.Once);
        Assert.True(userAddedCorrectly);
    }

    // Deve permitir listar todos os funcionários
    [Fact]
    public async Task ListUsers_WhenFilterIsCorrect_ShouldReturnListOfUsers()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";

        User user = new User(userName, emailCreated, bio);

        _userRepositoryMock.Setup(_userRepositoryMock => _userRepositoryMock.GetAllAsync())
            .ReturnsAsync(new List<User> { user });

        //Act
        List<User> users = await _userService.ListAllUsers();

        //Assert
        _userRepositoryMock.Verify(_userRepositoryMock => _userRepositoryMock.GetAllAsync(), Times.Once);
        Assert.NotNull(users);
        Assert.Single(users);
    }

    // Deve permitir atualizar um funcionário existente
    [Fact]
    public async Task UpdateUser_WhenDatasAreCorrects_ShouldReturnTrue()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";

        User user = new User(userName, emailCreated, bio);

        string newUserName = "New User Name";
        string newBio = "New Bio Test";

        user.Name = newUserName;
        user.Bio = newBio;

        _userRepositoryMock
            .Setup(_userRespositoryMock => _userRespositoryMock.UpdateAsync(user))
            .ReturnsAsync(true);

        //Act
        bool userEffectiveUpdated = await _userService.Update(user);

        //Assert
        _userRepositoryMock.Verify(userRepository => userRepository.UpdateAsync(user), Times.Once());
    }

    // Deve permitir excluir um funcionário existente
    [Fact]
    public async Task DeleteUser_WhenIdIsCorrect_ShouldReturnTrue()
    {
        //Arrange
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        string userName = "Test Name";
        string bio = "Bio Test";

        User user = new User(userName, emailCreated, bio);

        _userRepositoryMock
            .Setup(_userRepositoryMock => _userRepositoryMock.DeleteAsync(user.Id))
            .ReturnsAsync(true);

        //Act
        bool userCorrectlyDelete = await _userService.Delete(user.Id);

        //Assert
        _userRepositoryMock.Verify(userRepository => userRepository.DeleteAsync(user.Id), Times.Once());
        Assert.True(userCorrectlyDelete);
    }
}
