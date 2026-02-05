using Azure;
using Blog.API.Dtos.Users;
using Blog.API.Tests.Fixtures;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Blog.API.Tests.Controllers;

public class UserControllerTests : IClassFixture<ApiTestFactory>
{

    private readonly HttpClient _client;

    public UserControllerTests(ApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostUsers_ShouldCreateUser_WhenRequestIsValid()
    {
        //Arrange
        var request = new CreateUserRequest
        {
            Name = "John Doe",
            Email = "test@email.com",
            Bio = "This is a bio"
        };

        //Act
        var response = await _client.PostAsJsonAsync("/api/users", request);
        
        //Assert
        var content = await response.Content.ReadAsStringAsync();   
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    //POST /users com email inválido retorna 400
    [Fact]
    public async Task PostUsers_ShouldReturnError_WhenEmailIsIncorrect()
    {
        string incorrectEmail = "testemailcom";

        //Arrange
        var request = new CreateUserRequest
        {
            Name = "John Doe",
            Email = incorrectEmail,
            Bio = "This is a bio"
        };

        //Act
        var response = await _client.PostAsJsonAsync("/api/users", request);

        //Assert
        var context = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("\"Email\"");
    }

    //POST /users com email duplicado retorna
    [Fact]
    public async Task PostUsers_ShouldReturnError_WhenEmailIsDuplicated()
    {
        //Arrange
        string email = "dubemail@test.com";

        var resquest = new CreateUserRequest
        {
            Name = "John Doe",
            Email = email,
            Bio = "This is a bio"
        };

        var response = await _client.PostAsJsonAsync("/api/users", resquest);

        var resquest2 = new CreateUserRequest
        {
            Name = "John Doe II",
            Email = email,
            Bio = "This is a bio II"
        };

        //Act
        var response2 = await _client.PostAsJsonAsync("/api/users", resquest2);

        //Assert
        response2.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }


    //GET /users/{id} retorna usuário existente
    [Fact]
    public async Task GetUsers_ShouldReturnUser_WhenIdExists()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Name = "John Doe",
            Email = "email@test.com",
            Bio = "This is a bio"
        };

        // Act – cria usuário
        var postResponse = await _client.PostAsJsonAsync("/api/users", request);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Lê o usuário criado
        var createdUser = await postResponse.Content
            .ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        var getResponse = await _client.GetAsync($"/api/users/{createdUser!.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await getResponse.Content
            .ReadFromJsonAsync<UserResponse>();

        user.Should().NotBeNull();
        user!.Name.Should().Be("John Doe");
        user.Email.Should().Be("email@test.com");
    }

    //GET /users/{id} inexistente retorna 404
    [Fact]
    public async Task GetUsers_ShouldReturn404_WhenIdDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var getResponse = await _client.GetAsync($"/api/users/{nonExistentId}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    //PUT /users/{id} atualiza bio
    [Fact]
    public async Task PutUsers_ShouldUpdate_WhenDatasAreCorrect()
    {
        // Arrange
        var userRequest = new CreateUserRequest
        {
            Name = "John Doe",
            Email = "email@test.com",
            Bio = "This is a bio"
        };

        var postResponse = await _client.PostAsJsonAsync("/api/users", userRequest);
        
        var userResponse = await postResponse.Content
            .ReadFromJsonAsync<UserResponse>();

        UpdateUserRequest updateUserRequest = new UpdateUserRequest
        {
            Name = "John Doe",
            Email = "email@test.com",
            Bio = "New Bio"
        };

        // Act
        var userIsUpdated = await _client.PutAsJsonAsync($"/api/users/{userResponse!.Id}", updateUserRequest);

        // Assert
        userIsUpdated.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    //PUT /users/{id} inexistente retorna 404
    [Fact]
    public async Task PutUsers_ShouldReturnError_WhenIdNotExists()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        UpdateUserRequest updateUserRequest = new UpdateUserRequest
        {
            Name = "John Doe",
            Email = "email@test.com",
            Bio = "New Bio"
        };

        //Act
        var userIsUpdated = await _client.PutAsJsonAsync($"/api/users/{nonExistentId}", updateUserRequest);

        //Assert
        userIsUpdated.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    //GET /users retorna lista paginada
    [Fact]
    public async Task ListUsers_ShouldListAllUsers_WhenAreAllCorrect()
    {
        //Arrange
        var userRequest1 = new CreateUserRequest
        {
            Name = "John Doe 1",
            Email = "email1@test.com",
            Bio = "This is a bio 1"
        };
        var userRequest2 = new CreateUserRequest
        {
            Name = "John Doe 2",
            Email = "email2@test.com",
            Bio = "This is a bio 2"
        };
        var userRequest3 = new CreateUserRequest
        {
            Name = "John Doe 3",
            Email = "email3@test.com",
            Bio = "This is a bio 3"
        };

        await _client.PostAsJsonAsync("/api/users", userRequest1);
        await _client.PostAsJsonAsync("/api/users", userRequest2);
        await _client.PostAsJsonAsync("/api/users", userRequest3);

        //Act
        var response = await _client.GetAsync("/api/users");

        //Assert
        response.EnsureSuccessStatusCode();

        var users = await response.Content
            .ReadFromJsonAsync<List<UserResponse>>();

        users.Should().NotBeNull();
        users.Should().HaveCount(3);
    }
}
