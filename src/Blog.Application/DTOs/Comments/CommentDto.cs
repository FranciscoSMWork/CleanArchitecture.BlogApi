
using Blog.Domain.Entities;

namespace Blog.Application.DTOs.Comments;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid PostId { get; set; }
    public Post Post {  get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
}
