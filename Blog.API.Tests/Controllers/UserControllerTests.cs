using Blog.API.Tests.Fixtures;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
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
        // Arrange
        var request = new
        {
            name = "Name Test",
            email = new Email("test@email.com"),
            bio = "Bio Test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().NotBeNullOrEmpty();

    }
}
