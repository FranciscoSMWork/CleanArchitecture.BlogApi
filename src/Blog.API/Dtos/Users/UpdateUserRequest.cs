using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos.Users;

public class UpdateUserRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [StringLength(500)]
    public string Bio { get; set; } = null!;
}
