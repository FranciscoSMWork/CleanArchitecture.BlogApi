namespace Blog.Application.DTOs.Users;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
}
