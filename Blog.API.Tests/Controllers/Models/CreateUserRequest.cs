namespace Blog.API.Tests.Controllers.Models;

public class CreateUserRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Bio { get; set; }
}
