using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos.Comments;

public class CreateCommentRequest
{
    [Required]
    public Guid PostId { get; set; }
    [Required]
    public Guid AuthorId { get; set; }
    [Required]
    [StringLength(300, MinimumLength = 2)]
    public string Content { get; set; }
}
