using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos.Posts;
public class UpdatePostRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; }
 
    [Required]
    [StringLength(500, MinimumLength = 2)]
    public string? Content { get; set; }
    
    [Required]
    public Guid? AuthorId { get; set; }
}
