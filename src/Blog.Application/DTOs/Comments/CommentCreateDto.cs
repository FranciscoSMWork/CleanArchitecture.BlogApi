namespace Blog.Application.DTOs.Comments;

public class CommentCreateDto
{
    public Guid PostId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; }
}
