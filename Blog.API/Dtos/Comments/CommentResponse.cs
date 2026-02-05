using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;

namespace Blog.API.Dtos.Comments;

public class CommentResponse
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public PostResponse Post{get; set;}
    public Guid AuthorId { get; set; }
    public UserResponse Author { get; set; }
    public string Content { get; set; }
}
