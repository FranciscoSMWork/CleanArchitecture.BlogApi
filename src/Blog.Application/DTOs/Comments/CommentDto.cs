
using Blog.Application.DTOs.Posts;
using Blog.Application.DTOs.Users;

namespace Blog.Application.DTOs.Comments;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid PostId { get; set; }
    public PostDto Post {  get; set; }
    public Guid AuthorId { get; set; }
    public UserDto Author { get; set; }
}
