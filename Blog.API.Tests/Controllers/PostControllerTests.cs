
using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;
using Blog.API.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Blog.API.Tests.Controllers;

public class PostControllerTests : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client;

    public PostControllerTests(ApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    //POST /posts cria post válido
    [Fact]
    public async Task CreatePost_ValidPost_ReturnsCreatedPost()
    {
        // Arrange
        string Title = "Sample Post Title";
        string Content = "This is the content of the sample post.";

        CreateUserRequest createUserRequest = new CreateUserRequest
        {
            Name = "Jane Doe",
            Email = "email@test.com",
            Bio = "This is a sample bio."
        };

        var userResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        userResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdUser = await userResponse.Content
            .ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);
        
        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        var body = await response.Content.ReadAsStringAsync();
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    //POST /posts sem título retorna 400
    [Fact]
    public async Task CreatePost_PostWithoutTitle_ShouldReturnError()
    {
        //Arrange
        string Title = "";
        string Content = "This is the content of the sample post.";

        CreateUserRequest createUserRequest = new CreateUserRequest
        {
            Name = "Jane Doe",
            Email = "email@test.com",
            Bio = "This is a sample bio."
        };

        var userResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        userResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdUser = await userResponse.Content
            .ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        //Act
        var response = await _client.PostAsJsonAsync("/api/posts", createPostRequest);

        //Assert
        await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    //POST /posts com autor inexistente retorna 400
    [Fact]
    public async Task CreatePost_PostWithNonexistentAuthor_ShouldReturnError()
    {
        //Arrange
        string Title = "Sample Post Title";
        string Content = "This is the content of the sample post.";

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = Guid.NewGuid()
        };

        //Act
        var response = await _client.PostAsJsonAsync("/api/posts", createPostRequest);

        //Assert
        var body = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body.Should().Contain("Author");
    }

    //GET /posts retorna lista
    [Fact]
    public async Task GetPost_ListAllPost()
    {
        //Arrange
        string Title = "Sample Post Title";
        string Content = "This is the content of the sample post.";

        CreateUserRequest createUserRequest = new CreateUserRequest
        {
            Name = "Jane Doe",
            Email = "email@test.com",
            Bio = "This is a sample bio."
        };

        var userResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        userResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdUser = await userResponse.Content
            .ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        await _client.PostAsJsonAsync("/api/posts", createPostRequest);

        //Act
        var response = await _client.GetAsync("/api/posts");

        //Assert
        response.EnsureSuccessStatusCode();

        var posts = await response.Content
            .ReadFromJsonAsync<List<PostResponse>>();

        posts.Should().NotBeNull();
        posts.Should().HaveCount(3);
    }

    //GET /posts/{id} retorna post existente
    [Fact]
    public async Task GetPost_ById_ShouldReturnsExistingPost()
    {
        //Arrange
        string Title = "Sample Post Title";
        string Content = "This is the content of the sample post.";

        CreateUserRequest createUserRequest = new CreateUserRequest
        {
            Name = "Jane Doe",
            Email = "email@test.com",
            Bio = "This is a sample bio."
        };

        var userResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        userResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdUser = await userResponse.Content
            .ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        var response = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdPost = await response.Content.ReadFromJsonAsync<PostResponse>();

        //Act
        var getResponse = await _client.GetAsync($"/api/posts/{createdPost!.Id}");

        //Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var post = await getResponse.Content
            .ReadFromJsonAsync<PostResponse>();

        post.Title.Should().Be(Title);
        post.Content.Should().Be(Content);
        post.AuthorId.Should().Be(createdUser.Id);
        post.Author.Name.Should().Be(createdUser.Name);
        post.Author.Email.Should().Be(createdUser.Email);
        post.Author.Bio.Should().Be(createdUser.Bio);

    }

    //GET /posts/{id} inexistente retorna 404
    [Fact]
    public async Task GetPost_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        //Act
        var response = await _client.GetAsync($"/api/posts/{Guid.NewGuid()}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    //PUT /posts/{id} atualiza conteúdo
    [Fact]
    public async Task PutPost_ShouldUpdatePost_WhenDatesAreCorrect()
    {
        //Arrange
        CreateUserRequest createUserRequest = new CreateUserRequest
        {
            Name = "Jane Doe",
            Email = "email@test.com",
            Bio = "This is a sample bio."
        };

        var userResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        userResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdUser = await userResponse.Content
            .ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);
        
        string Title = "Sample Post Title";
        string Content = "This is the content of the sample post.";

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };
        
        var response = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        //Act
        string newTitle = "Updated Post Title";
        string newContent = "This is the updated content of the sample post.";

        UpdatePostRequest updatePostRequest = new UpdatePostRequest
        {
            Title = newTitle,
            Content = newContent,
            AuthorId = createdUser!.Id
        };

        var updatedPost = await _client.PutAsJsonAsync($"/api/posts/{createdUser.Id}", updatePostRequest);

        //Assert
        updatedPost.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedPost.Should().NotBeNull();
        
        var getResponse = await _client.GetAsync($"/api/posts/{createdUser.Id}");
        var updatedPostContent = await getResponse.Content
           .ReadFromJsonAsync<PostResponse>();
        
        updatedPostContent.Title.Should().Be(newTitle);
        updatedPostContent.Content.Should().Be(newContent);
    }

    //DELETE /posts/{id} remove post
    [Fact]
    public async Task DeletePost_ShouldDestroyPost_WhenIdExists()
    {
        //Arrange
        CreateUserRequest createUserRequest = new CreateUserRequest
        {
            Name = "Jane Doe",
            Email = "email@test.com",
            Bio = "This is a sample bio."
        };

        var userResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        userResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdUser = await userResponse.Content
            .ReadFromJsonAsync<UserResponse>();


        string Title = "Sample Post Title";
        string Content = "This is the content of the sample post.";

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        var postCreatedResponse = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        postCreatedResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var postCreated = await postCreatedResponse.Content.ReadFromJsonAsync<PostResponse>();

        //Act
        var postIsDeleted = await _client.DeleteAsync($"/api/posts/{postCreated!.Id}");

        //Assert
        postIsDeleted.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    //DELETE /posts/{id} inexistente retorna 404
    [Fact]
    public async Task DeletePost_ShouldDestroyPost_WhenIdDoesnotExists()
    {
        //Act
        var postIsDeleted = await _client.DeleteAsync($"/api/posts/{Guid.NewGuid()}");

        //Assert
        postIsDeleted.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
