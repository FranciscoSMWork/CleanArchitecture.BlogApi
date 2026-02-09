using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos.Posts;
public class UpdatePostRequest
{
    [StringLength(100, MinimumLength = 2)]
    public string? Title { get; set; }
 
    [StringLength(500, MinimumLength = 2)]
    public string? Content { get; set; }
}
