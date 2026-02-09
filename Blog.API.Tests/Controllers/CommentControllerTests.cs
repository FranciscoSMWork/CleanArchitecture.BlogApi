
using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;
using Blog.API.Tests.Fixtures;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Blog.API.Dtos.Comments;
using Blog.Domain.Entities;

namespace Blog.API.Tests.Controllers;

public class CommentControllerTests
{
    private readonly HttpClient _client;
 
    public CommentControllerTests()
    {
        ApiTestFactory factory = new ApiTestFactory();
        _client = factory.CreateClient();
    }

    //POST /comments cria comentário válido
    [Fact]
    public async Task GetComment_CreateCommentShouldReturnCreatedComment_WhenDatasAreCorrect()
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
        var createdUser = await userResponse.Content.ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        var postResponse = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdPost = await postResponse.Content.ReadFromJsonAsync<PostResponse>();

        CreateCommentRequest createCommentRequest = new CreateCommentRequest
        {
            PostId = createdPost!.Id,
            AuthorId = createdUser.Id,
            Content = "This is a sample comment."
        };

        var commentResponse = await _client.PostAsJsonAsync("/api/comment", createCommentRequest);
        commentResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdComment = await commentResponse.Content.ReadFromJsonAsync<CommentResponse>();
        createdComment!.Id.Should().NotBe(Guid.Empty);
        createdComment.Author.Name.Should().Be(createUserRequest.Name);
    }


    //POST /comments vazio retorna 400
    [Fact]
    public async Task PostComment_ShouldReturnError_WhenContentIsEmpty()
    {
        // Arrange
        CreateUserRequest createUserRequest = new CreateUserRequest
        {
            Name = "Jane Doe",
            Email = "email@test.com",
            Bio = "This is a sample bio."
        };

        var userResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        userResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdUser = await userResponse.Content.ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        string Title = "Sample Post Title";
        string postContent = "Post Content";
        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = postContent,
            AuthorId = createdUser!.Id
        };

        var postResponse = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdPost = await postResponse.Content.ReadFromJsonAsync<PostResponse>();

        CreateCommentRequest createCommentRequest = new CreateCommentRequest
        {
            PostId = createdPost!.Id,
            AuthorId = createdUser.Id,
            Content = ""
        };

        var commentResponse = await _client.PostAsJsonAsync("/api/comment", createCommentRequest);
        commentResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var createdComment = await commentResponse.Content.ReadFromJsonAsync<CommentResponse>();

    }

    //POST /comments com post inexistente retorna 400
    [Fact]
    public async Task PostComment_ShouldReturnError_WhenPostDoesNotExist()
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
        var createdUser = await userResponse.Content.ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        var postResponse = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdPost = await postResponse.Content.ReadFromJsonAsync<PostResponse>();

        CreateCommentRequest createCommentRequest = new CreateCommentRequest
        {
            PostId = Guid.NewGuid(),
            AuthorId = createdUser.Id,
            Content = "This is a sample comment."
        };

        var commentResponse = await _client.PostAsJsonAsync("/api/comment", createCommentRequest);
        commentResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    //GET /posts/{id}/comments retorna lista
    [Fact]
    public async Task GetComment_ShouldReturnAListOfComments()
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
        var createdUser = await userResponse.Content.ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        var postResponse = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdPost = await postResponse.Content.ReadFromJsonAsync<PostResponse>();

        CreateCommentRequest createCommentRequest = new CreateCommentRequest
        {
            PostId = createdPost!.Id,
            AuthorId = createdUser.Id,
            Content = "This is a sample comment."
        };

        await _client.PostAsJsonAsync("/api/comment", createCommentRequest);
        await _client.PostAsJsonAsync("/api/comment", createCommentRequest);
        await _client.PostAsJsonAsync("/api/comment", createCommentRequest);

        var listCommentResponse = await _client.GetAsync("/api/comment");

        List<CommentResponse> listComment = await listCommentResponse.Content.ReadFromJsonAsync<List<CommentResponse>>();

        listComment.Should().NotBeNull();
        listComment.Should().HaveCount(3);
    }

    //DELETE /comments/{id} remove comentário
    public async Task DeleteComment_ShouldRemoveComment_WhenIdIsValid()
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
        var createdUser = await userResponse.Content.ReadFromJsonAsync<UserResponse>();

        createdUser!.Id.Should().NotBe(Guid.Empty);

        CreatePostRequest createPostRequest = new CreatePostRequest
        {
            Title = Title,
            Content = Content,
            AuthorId = createdUser!.Id
        };

        var postResponse = await _client.PostAsJsonAsync("/api/posts", createPostRequest);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdPost = await postResponse.Content.ReadFromJsonAsync<PostResponse>();

        CreateCommentRequest createCommentRequest = new CreateCommentRequest
        {
            PostId = createdPost!.Id,
            AuthorId = createdUser.Id,
            Content = "This is a sample comment."
        };

        var commentResponse = await _client.PostAsJsonAsync("/api/comments", createCommentRequest);
        commentResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdComment = await commentResponse.Content.ReadFromJsonAsync<CommentResponse>();
        

        var deleteResponse = await _client.DeleteAsync($"/api/comments/{createdComment!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
