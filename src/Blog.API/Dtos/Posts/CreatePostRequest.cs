using Blog.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos.Posts;

public class CreatePostRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(500, MinimumLength = 2)]
    public string Content { get; set; } = null!;
    
    [Required]
    public Guid AuthorId { get; set; }
}
