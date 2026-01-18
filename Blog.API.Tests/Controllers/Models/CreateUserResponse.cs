namespace Blog.API.Tests.Controllers.Models;

public class CreateUserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Bio { get; set; }
}
