using Blog.API.Dtos.Users;
using Blog.API.Tests.Fixtures;
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
        Console.WriteLine(response);
        //Assert
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);

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
}
