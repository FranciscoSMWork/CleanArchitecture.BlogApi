using Blog.API.Tests.Fixtures;

namespace Blog.API.Tests.Controllers;

public class CommentControllerTests : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client;

    public CommentControllerTests(ApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }
}
