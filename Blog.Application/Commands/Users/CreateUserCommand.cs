namespace Blog.Application.Commands.Users;

public class CreateUserCommand
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
}
